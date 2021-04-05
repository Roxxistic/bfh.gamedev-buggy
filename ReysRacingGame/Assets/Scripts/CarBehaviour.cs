using System.Collections;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    public WheelCollider wheelColliderFL;
    public WheelCollider wheelColliderFR;
    public float maxTorque = 500;
    public float maxSteerAngle = 45;

    // Start is called before the first frame update
    void Start()
    {}

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
}
