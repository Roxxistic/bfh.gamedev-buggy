using System;
using UnityEngine;
using System.Collections.Generic;

class Gear
{
    private float _minRPM;
    private float _minKMH;
    private float _maxRPM;
    private float _maxKMH;
    public readonly int GearNumber;
    private readonly float _slope;

    public Gear(int gearNumber, float minKMH, float minRPM, float maxKMH, float maxRPM)
    {
        _minRPM = minRPM;
        _minKMH = minKMH;
        _maxRPM = maxRPM;
        _maxKMH = maxKMH;
        GearNumber = gearNumber;
        _slope = (_maxRPM - _minRPM) / (_maxKMH - _minKMH);
    }
    

    public bool speedFits(float kmh)
    {
        return kmh >= _minKMH && kmh <= _maxKMH;
    }

    public float interpolate(float kmh)
    {
        return _minRPM + (kmh - _minKMH) * _slope;
    }
}

public class CarBehaviour : MonoBehaviour
{
    public WheelCollider wheelColliderFL;
    public WheelCollider wheelColliderFR;
    public WheelCollider wheelColliderBL;
    public WheelCollider wheelColliderBR;
    public Transform centerOfMass;

    public WheelBehaviour[] wheelBehaviours = new WheelBehaviour[4];

    private Rigidbody _rigidBody;

    private List<Gear> _gears = new List<Gear> {
        new Gear(1, 1, 900, 12, 1400),
        new Gear(2, 12, 900, 25, 2000),
        new Gear(3, 25, 1350, 45, 2500),
        new Gear(4, 45, 1950, 70, 3500),
        new Gear(5, 70, 2500, 112, 4000),
        new Gear(6, 112, 3100, 180, 5000)
    };

    public float sidewaysStiffness = 1.5f;
    public float forewardStiffness = 1.5f;
    public float maxTorque = 500;
    public float maxSteerAngle = 45;
    public float maxSpeedKMH = 150;
    public float maxSpeedBackwardKMH = 1;
    public float speedMeterMaxSpeed = 140f;
    public float speedMeterRotationOffset = 34f;
    public float maxBrakeTorque = 500;

    public Transform water;
	public float WaterLevel => water != null ? water.position.y : 0f;
    public float buggyHeight = 2.0f;
    /**
     * Defines the maximum amount of water resistance the buggy experiences as BrakeTorque.
     */
    public float maxBreakTorqueDueToRelativeSubmersion = 200;

    /**
     * Returns a value [0, 1] where 0 is none submersion under water and 1 is full submersion under water.
     */
    public float BuggySubmersion {
        get {
            var submersionInMeters = WaterLevel - transform.position.y;
            var relativeSubmersion = submersionInMeters / buggyHeight;
            float relativeValueFrom0To1;

            if(relativeSubmersion > 1)
			{
                relativeValueFrom0To1 = 1f;
			} else if(relativeSubmersion <= 0)
			{
                relativeValueFrom0To1 = 0f;
			} else
			{
                relativeValueFrom0To1 = relativeSubmersion;
			}

            Debug.Log($"Buggy is submerged underwater by {relativeValueFrom0To1}");

            return relativeValueFrom0To1;
        }
    }

    /**
     * Additional BrakeTorque if buggy is partially or fully underwater.
     * If above water, then is 0;
     * If fully in water, then experiences depth-independent water resistance.
     * If partially in water, then experiences depth-dependent water resistance.
     */
    public float CurrentBreakTorqueDueToRelativeSubmersion => maxBreakTorqueDueToRelativeSubmersion * BuggySubmersion;
    public float maxDragDueToSubmersion = 2;
    public float CurrentDragDueToRelativeSubmersion => maxDragDueToSubmersion * BuggySubmersion;

	public float torque = 0.0f;
        
    private bool thrustEnabled = false;


    public float CurrentSpeedKMH => _rigidBody.velocity.magnitude * 3.6f;
    public float SteerAngle { get; private set; }
    public bool MovingForwards => _rigidBody.velocity.z > 0;
    public bool MovingBackwards => _rigidBody.velocity.z < 0;
    // Determine if the car is driving forwards or backwards
    public bool VelocityIsForeward => Vector3.Angle(transform.forward, _rigidBody.velocity) < 50f;
    // Determine if the cursor key input means braking
    public bool DoBraking => CurrentSpeedKMH > 0.5f && (Input.GetAxis("Vertical") < 0 && VelocityIsForeward || Input.GetAxis("Vertical") > 0 && !VelocityIsForeward);
    public bool DoFullBrake => Input.GetKey("space");

    Gear CurrentGear => _gears.Find(g => g.speedFits(CurrentSpeedKMH)) ?? _gears.Find(g => g.GearNumber == 1);
    public float CurrentSpeedRPM => CurrentGear.interpolate(CurrentSpeedKMH);
    public int CurrentGearNumber => CurrentGear.GearNumber;

    public TimingCountdownBehaviour timingBehaviour;

    public bool _carIsOnDrySand;
    private  string _groundTagFL;
    private int _groundTextureFL;
    private string _groundTagFR;
    private int _groundTextureFR;

    // Full breaking and skidmarking
    public float fullBrakeTorque = 5000; 
    public AudioClip brakeAudioClip;
    private bool _doSkidmarking => _carIsNotOnSand && DoFullBrake && CurrentSpeedKMH > 20.0f; 
    private bool _carIsNotOnSand;
    private AudioSource _brakeAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.centerOfMass = centerOfMass.localPosition;

        SetWheelFrictionStiffness();

        // Configure brake audiosource component by program
        _brakeAudioSource = (AudioSource)gameObject.AddComponent<AudioSource>(); 
        _brakeAudioSource.clip = brakeAudioClip; 
        _brakeAudioSource.loop = true; 
        _brakeAudioSource.volume = 0.7f; 
        _brakeAudioSource.playOnAwake = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

            // Evaluate ground under front wheels
            WheelHit hitFL = GetGroundInfos(ref wheelColliderFL, ref _groundTagFL, ref _groundTextureFL);
            WheelHit hitFR = GetGroundInfos(ref wheelColliderFR, ref _groundTagFR, ref _groundTextureFR);
            _carIsOnDrySand = _groundTagFL.CompareTo("Terrain") == 0 && _groundTextureFL == 3;
            _carIsNotOnSand = !(_groundTagFL.CompareTo("Terrain") == 0 && (_groundTextureFL <= 1));

            SetThrustEnabled();
            SetBrakeTorque();
            SetBrakeSound(_doSkidmarking);
            SetSkidmarking(_doSkidmarking);
            SetMotorTorque();
            SetSteerAngle();
            SlowDownInWater();
            
    }

	public void FreezeAfterRescue()
	{
        //Vector3 counterMovement = new Vector3(-_rigidBody.velocity.x, 0, -_rigidBody.velocity.z);
        //_rigidBody.AddForce(counterMovement);

        _rigidBody.velocity = Vector3.zero;

        torque = 0;

        wheelColliderFL.motorTorque = 0;
        wheelColliderFR.motorTorque = 0;

        wheelColliderFL.brakeTorque = Mathf.Infinity;
        wheelColliderFR.brakeTorque = Mathf.Infinity;
        wheelColliderBL.brakeTorque = Mathf.Infinity;
        wheelColliderBR.brakeTorque = Mathf.Infinity;
    }

	private void SetThrustEnabled()
	{
        thrustEnabled = timingBehaviour.IsCountdownDone;
    }

    private void SetMotorTorque()
	{
        torque = 0;

        bool maxForwardSpeedNotExceeded = VelocityIsForeward && CurrentSpeedKMH < maxSpeedKMH;
        bool maxBackwardSpeedNotExceeded = !VelocityIsForeward && CurrentSpeedKMH < maxSpeedBackwardKMH;

        if ((maxBackwardSpeedNotExceeded || maxForwardSpeedNotExceeded) && !DoBraking && !DoFullBrake && thrustEnabled)
        {
            torque = maxTorque * Input.GetAxis("Vertical");
        }

        wheelColliderFL.motorTorque = torque;
        wheelColliderFR.motorTorque = torque;
    }

    private void SetBrakeTorque()
	{
        float brakeTorque = 0;
		if (DoFullBrake)
		{
            brakeTorque = fullBrakeTorque;
		}
		if (DoBraking)
		{
            brakeTorque = maxBrakeTorque;
		}

        wheelColliderFL.brakeTorque = brakeTorque;
        wheelColliderFR.brakeTorque = brakeTorque;
        wheelColliderBL.brakeTorque = brakeTorque;
        wheelColliderBR.brakeTorque = brakeTorque;
    }

    private void SlowDownInWater()
	{
        if (BuggySubmersion <= 0) return;

        _rigidBody.drag = CurrentDragDueToRelativeSubmersion;
        Debug.Log($"current drag {CurrentDragDueToRelativeSubmersion}");
	}

    private void SetBrakeSound(bool doBrakeSound)
	{
        if (doBrakeSound)
        {
            _brakeAudioSource.volume = CurrentSpeedKMH / 100.0f;
            _brakeAudioSource.Play();
        }
        else _brakeAudioSource.Stop();
    }

    private void SetSkidmarking(bool doSkidmarking)
	{
        foreach (var wheel in wheelBehaviours)
		{
            wheel.DoSkidmarking(doSkidmarking);
        }
    }

    private void SetSteerAngle()
	{
        SteerAngle = maxSteerAngle * Input.GetAxis("Horizontal");

        // Adapt steering intensity.
        // The faster the car, the less it can steer to left/right. 
        // Current speed to Max speed ratio, inverted (1-value) to get a smaller number when current speed is higher.
        // Use 1.1f instead of 1f, so when current speed is equal to max speed, angleLock is not 0 (which would prevent the car from steering at max speed).
        // Also apply Min(1, value), to not get a value above 1, should the current speed be very low.
        float angleLock = Math.Min(1.1f - (CurrentSpeedKMH / maxSpeedKMH), 1);
        SteerAngle *= angleLock;

        wheelColliderFL.steerAngle = SteerAngle;
        wheelColliderFR.steerAngle = SteerAngle;
	}

    private void SetWheelFrictionStiffness()
	{
        // Leider lässt sich das stiffness-Property nicht direkt setzen, sodass wir über ein WheelFrictionCurve Objekt gehen müssen.
        WheelFrictionCurve f_fwWFC = wheelColliderFL.forwardFriction; 
        WheelFrictionCurve f_swWFC = wheelColliderFL.sidewaysFriction;
        
        f_fwWFC.stiffness = forewardStiffness; 
        f_swWFC.stiffness = sidewaysStiffness;
        
        wheelColliderFL.forwardFriction = f_fwWFC;
        wheelColliderFL.sidewaysFriction = f_swWFC;
        wheelColliderFR.forwardFriction = f_fwWFC;
        wheelColliderFR.sidewaysFriction = f_swWFC;
        
        wheelColliderBL.forwardFriction = f_fwWFC;
        wheelColliderBL.sidewaysFriction = f_swWFC;
        wheelColliderBR.forwardFriction = f_fwWFC;
        wheelColliderBR.sidewaysFriction = f_swWFC;
    }

    // Returns the wheel hit collider, the tag and main texture of the passed wheel collider
    WheelHit GetGroundInfos(ref WheelCollider wheelCol, ref string groundTag, ref int groundTextureIndex)
	{
        // Default values
        groundTag = "InTheAir";
        groundTextureIndex = -1;

        // Query ground by ray shoot on the front left wheel collider
        WheelHit wheelHit;
        wheelCol.GetGroundHit(out wheelHit);

        // If not in the air query collider
        if (wheelHit.collider)
		{
            groundTag = wheelHit.collider.tag;
			if (wheelHit.collider.CompareTag("Terrain"))
			{
                groundTextureIndex = TerrainSurface.GetMainTexture(transform.position);
			}
		}
        return wheelHit;
	}
}

public class TerrainSurface
{
    public static float[] GetTextureMix(Vector3 worldPosition)
	{
        Terrain terrain = Terrain.activeTerrain;
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPosition = terrain.transform.position;

        // calculate which splat map cell the worldPos falls within (ignoring y)
        int mapX = (int)(((worldPosition.x- terrainPosition.x)/ terrainData.size.x) * terrainData.alphamapWidth);
        int mapZ = (int)(((worldPosition.z- terrainPosition.z)/ terrainData.size.z) * terrainData.alphamapHeight);
        // get the splat data for this cell as 1x1xN 3d array (where N=number of textures)
        float[,,] splatMapData = terrainData.GetAlphamaps(mapX,mapZ,1,1);
        // extract the 3D array data to a 1D array:
        float[] cellMix = new float[splatMapData.GetUpperBound(2) + 1];
        for (int n = 0; n < cellMix.Length; ++n) cellMix[n] = splatMapData[0, 0, n];
        return cellMix;
    }

    public static int GetMainTexture(Vector3 worldPos)
    {
        float[] mix = GetTextureMix(worldPos);
        float maxMix = 0;
        int maxIndex = 0;

        // loop through each mix value and find the maximum
        for (int n=0; n < mix.Length; ++n) {
            if (mix[n] > maxMix) {
                maxIndex = n;
                maxMix = mix[n];
            }
        }
        return maxIndex;
    }
}
