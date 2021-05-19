using UnityEngine;

public class CarStopwatchBehaviour : MonoBehaviour
{
	public float PastTime { get; private set; }

	public bool IsRunning = false;

	public void StartStopwatch()
	{
		IsRunning = true;
	}

	public void StopStopwatch()
	{
		IsRunning = false;
	}

	private void OnGUI()
	{
		if (IsRunning)
		{
			PastTime += Time.deltaTime;
		}
	}
}
