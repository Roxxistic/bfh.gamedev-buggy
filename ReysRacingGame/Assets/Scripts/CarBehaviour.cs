using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class CarBehaviour : MonoBehaviour
{
    public WheelCollider wheelColliderFL;
    public WheelCollider wheelColliderFR;
    public WheelCollider wheelColliderBL;
    public WheelCollider wheelColliderBR;
    public Transform centerOfMass;
    public float maxTorque = 500;
    public float maxSteerAngle = 45;
    public float sidewaysStiffness = 1.5f;
    public float forewardStiffness = 1.5f;
    public float maxSpeedKMH = 150;
    public float maxSpeedBackwardKMH = 1;
    public RectTransform speedPointerTransform;
    public TMP_Text speedText;
    public float speedMeterMaxSpeed = 150;

    private Rigidbody _rigidBody;
    private float _currentSpeedKMH;
    private bool _movingForwards => _rigidBody.velocity.z > 0;
    private bool _movingBackwards => _rigidBody.velocity.z < 0;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.centerOfMass = centerOfMass.localPosition;

        SetWheelFrictionStiffness(forewardStiffness, sidewaysStiffness);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _currentSpeedKMH = _rigidBody.velocity.magnitude * 3.6f;

        // Determine if the car is driving forwards or backwards
        bool velocityIsForeward = Vector3.Angle(transform.forward, _rigidBody.velocity) < 50f;
        // Determine if the cursor key input means braking
        bool doBraking = _currentSpeedKMH > 0.5f && (Input.GetAxis("Vertical") < 0 && velocityIsForeward || Input.GetAxis("Vertical") > 0 && !velocityIsForeward);
        if (doBraking)
        {
            wheelColliderFL.brakeTorque = 5000;
            wheelColliderFR.brakeTorque = 5000;
            wheelColliderBL.brakeTorque = 5000;
            wheelColliderBR.brakeTorque = 5000;
            wheelColliderFL.motorTorque = 0;
            wheelColliderFR.motorTorque = 0;
        }
        else
        {
            wheelColliderFL.brakeTorque = 0;
            wheelColliderFR.brakeTorque = 0;
            wheelColliderBL.brakeTorque = 0;
            wheelColliderBR.brakeTorque = 0;

            // TODO: limit velocity
            if((velocityIsForeward && _currentSpeedKMH < maxSpeedKMH) || (!velocityIsForeward && _currentSpeedKMH < maxSpeedBackwardKMH)) {
                wheelColliderFL.motorTorque = maxTorque * Input.GetAxis("Vertical");
                
                wheelColliderFR.motorTorque = wheelColliderFL.motorTorque;
            } else
			{
                wheelColliderFL.motorTorque = 0;
                wheelColliderFR.motorTorque = 0;
            }
        }

        //SetMotorTorque(maxTorque * Input.GetAxis("Vertical"));
        SetSteerAngle(maxSteerAngle * Input.GetAxis("Horizontal"));
    }

	private void OnGUI()
	{
        // Speedpointer rotation
        float degAroundZ = 34f + _currentSpeedKMH / speedMeterMaxSpeed * (360f - 34f - 34f);
        speedPointerTransform.rotation = Quaternion.Euler(0f,0f, -degAroundZ);
        // SpeedText show current KMH
        speedText.text = $"{_currentSpeedKMH:0} km/h";
    }

    void SetSteerAngle(float angle)
	{
        angle = AdaptSteeringIntensity(angle);

        wheelColliderFL.steerAngle = angle;
        wheelColliderFR.steerAngle = angle;
	}

    void SetMotorTorque(float amount)
	{
        //amount = LimitSpeed(amount);

        wheelColliderFL.motorTorque = amount;
        wheelColliderFR.motorTorque = amount;
	}

    void SetWheelFrictionStiffness(float forewardStiffness, float sidewaysStiffness)
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

    float LimitSpeed(float nextSpeed)
	{
        Debug.Log($"Forward: {_movingForwards}. Backward: {_movingBackwards}. CurrentSpeed: {_currentSpeedKMH}. NextSpeed: {nextSpeed}.");
        if (_movingForwards && nextSpeed > 0 && _currentSpeedKMH >= maxSpeedKMH)
		{
            Debug.Log("Reduce forward");
            return 0f;
		}
        if(_movingBackwards && nextSpeed < 0 && _currentSpeedKMH >= maxSpeedBackwardKMH)
		{
            Debug.Log("Reduce backward");
            return 0f;
		}
        Debug.Log("Reduce none");
        return nextSpeed;
	}

    float AdaptSteeringIntensity(float angle)
	{
        // The faster the car, the less it can steer to left/right. 
        // Current speed to Max speed ratio, inverted (1-value) to get a smaller number when current speed is higher.
        // Use 1.1f instead of 1f, so when current speed is equal to max speed, angleLock is not 0 (which would prevent the car from steering at max speed).
        // Also apply Min(1, value), to not get a value above 1, should the current speed be very low.
        float angleLock = Math.Min(1.1f - (_currentSpeedKMH / maxSpeedKMH), 1);
        return angle * angleLock;
    }
}
