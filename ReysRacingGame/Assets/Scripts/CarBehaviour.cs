using System.Collections;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    public WheelCollider wheelColliderFL;
    public WheelCollider wheelColliderFR;
    public WheelCollider wheelColliderBL;
    public WheelCollider wheelColliderBR;
    public float maxTorque = 500;
    public float maxSteerAngle = 45;
    public float sidewaysStiffness = 1.5f;
    public float forewardStiffness = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        SetWheelFrictionStiffness(forewardStiffness, sidewaysStiffness);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetMotorTorque(maxTorque * Input.GetAxis("Vertical"));
        SetSteerAngle(maxSteerAngle * Input.GetAxis("Horizontal"));
    }

    void SetSteerAngle(float angle)
	{
        wheelColliderFL.steerAngle = angle;
        wheelColliderFR.steerAngle = angle;
	}

    void SetMotorTorque(float amount)
	{
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
}
