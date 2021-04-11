using UnityEngine;

public class CarFrontlightBehaviour : MonoBehaviour
{
    public GameObject buggy;
    /**
     * Horizontal Angle in degrees, where buggy frontal is 0°.
     */
    public float minAngle;

    CarBehaviour carBehaviour;

	private void Start()
	{
        carBehaviour = buggy.GetComponent<CarBehaviour>();
	}

	void Update()
    {
        AdjustFrontlightAngle();
    }

    private void AdjustFrontlightAngle()
	{
        float currentSteeringAngle = carBehaviour.SteerAngle;
        float buggyWorldRotation = buggy.transform.eulerAngles.y;

        float targetAngle = buggyWorldRotation + minAngle + 0.5f * currentSteeringAngle;

        Quaternion currentQuat = transform.rotation;
        float x = currentQuat.eulerAngles.x;
        float y = targetAngle;
        float z = currentQuat.eulerAngles.z;

        Quaternion nextQuat = Quaternion.Euler(x, y, z);
        transform.rotation = nextQuat;
    }
}
