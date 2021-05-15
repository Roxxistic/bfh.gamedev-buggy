﻿using UnityEngine;
using TMPro;

public class SpeedMeterBehaviour : MonoBehaviour
{
    public GameObject buggy;
    public RectTransform speedPointerTransform;
    public TMP_Text speedText;

    private CarBehaviour _carBehaviour;   

    public float speedMeterMaxSpeed = 140f;
    public float speedMeterRotationOffset = 34f;

    void Start()
    {
        _carBehaviour = buggy.GetComponent<CarBehaviour>();
    }

    private void OnGUI()
	{
        SetPointerPosition();
        SetSpeedText();
	}

    private void SetPointerPosition()
    {
        float degAroundZ = speedMeterRotationOffset + _carBehaviour.CurrentSpeedKMH / speedMeterMaxSpeed * (360f - speedMeterRotationOffset - speedMeterRotationOffset);
        speedPointerTransform.rotation = Quaternion.Euler(0f, 0f, -degAroundZ);
    }

    private void SetSpeedText()
    {
        speedText.text = $"{_carBehaviour.CurrentSpeedKMH:0} km/h";
    }

    private void SetStopWatch()
	{
        // TODO!
	}
}