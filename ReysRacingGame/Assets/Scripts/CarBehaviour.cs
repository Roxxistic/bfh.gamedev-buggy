﻿using System;
using UnityEngine;
using TMPro;

public class CarBehaviour : MonoBehaviour
{
    public WheelCollider wheelColliderFL;
    public WheelCollider wheelColliderFR;
    public WheelCollider wheelColliderBL;
    public WheelCollider wheelColliderBR;
    public Transform centerOfMass;

    private Rigidbody _rigidBody;

    public float sidewaysStiffness = 1.5f;
    public float forewardStiffness = 1.5f;
    public float maxTorque = 500;
    public float maxSteerAngle = 45;
    public float maxSpeedKMH = 150;
    public float maxSpeedBackwardKMH = 1;
    public float speedMeterMaxSpeed = 140f;
    public float speedMeterRotationOffset = 34f;
    public bool thrustEnabled = false;

    public float CurrentSpeedKMH => _rigidBody.velocity.magnitude * 3.6f;
    public float SteerAngle { get; private set; }
    public bool MovingForwards => _rigidBody.velocity.z > 0;
    public bool MovingBackwards => _rigidBody.velocity.z < 0;
    // Determine if the car is driving forwards or backwards
    public bool VelocityIsForeward => Vector3.Angle(transform.forward, _rigidBody.velocity) < 50f;
    // Determine if the cursor key input means braking
    public bool DoBraking => CurrentSpeedKMH > 0.5f && (Input.GetAxis("Vertical") < 0 && VelocityIsForeward || Input.GetAxis("Vertical") > 0 && !VelocityIsForeward);

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.centerOfMass = centerOfMass.localPosition;

        SetWheelFrictionStiffness();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetBrakeTorque();
        SetMotorTorque();
        SetSteerAngle();
    }

    private void SetMotorTorque()
	{
        float torque = 0;

        bool maxForwardSpeedNotExceeded = VelocityIsForeward && CurrentSpeedKMH < maxSpeedKMH;
        bool maxBackwardSpeedNotExceeded = !VelocityIsForeward && CurrentSpeedKMH < maxSpeedBackwardKMH;

        if ((maxBackwardSpeedNotExceeded || maxForwardSpeedNotExceeded) && !DoBraking && thrustEnabled)
        {
            torque = maxTorque * Input.GetAxis("Vertical");
        }

        wheelColliderFL.motorTorque = torque;
        wheelColliderFR.motorTorque = torque;
    }

    private void SetBrakeTorque()
	{
        float torque = DoBraking ? 5000 : 0;

        wheelColliderFL.brakeTorque = torque;
        wheelColliderFR.brakeTorque = torque;
        wheelColliderBL.brakeTorque = torque;
        wheelColliderBR.brakeTorque = torque;
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
}
