using UnityEngine;
using TMPro;

public class SpeedMeterBehaviour : MonoBehaviour
{
    public GameObject buggy;
    public RectTransform speedPointerTransform;
    public TMP_Text speedText;
    public TMP_Text timeText;
    public TimingCountdownBehaviour countdownBehaviour;

    private CarStopwatchBehaviour _carStopwatch;
    private CarBehaviour _carBehaviour;   

    public float speedMeterMaxSpeed = 140f;
    public float speedMeterRotationOffset = 34f;

    void Start()
    {
        _carBehaviour = buggy.GetComponent<CarBehaviour>();
        _carStopwatch = buggy.GetComponent<CarStopwatchBehaviour>();
    }

    private void OnGUI()
	{
        SetPointerPosition();
        SetSpeedText();
        SetTimeText();
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

    private void SetTimeText()
	{
        if (!countdownBehaviour.IsCountdownDone) {
            timeText.text = $"{countdownBehaviour.CountDown} sec.";
        }

        if(countdownBehaviour.IsCountdownDone && !_carStopwatch.IsRunning && _carStopwatch.PastTime < 1)
		{
            timeText.text = $"GO!";
        }

        if (_carStopwatch.IsRunning)
        {
            timeText.text = $"{_carStopwatch.PastTime:0.0} sec.";
        }
    }
}
