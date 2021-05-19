using UnityEngine;

public class GateStopwatchBehaviour : MonoBehaviour
{
	public bool IsStartGate = false;
	public bool IsEndGate = false;

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Car"))
		{
			Debug.Log("gate collision with Car");

			var carStopwatch = other.GetComponentInParent<CarStopwatchBehaviour>();

			if (IsStartGate && !carStopwatch.IsRunning)
			{
				carStopwatch.StartStopwatch();
				return;
			}
			
			if(IsEndGate && carStopwatch.IsRunning)
			{
				carStopwatch.StopStopwatch();
				return;
			}
		}
	}
}
