using UnityEngine;

public class CarFrontlightBehaviour : MonoBehaviour
{
    public GameObject buggy;
    public float MinAngle;

    CarBehaviour carBehaviour;

	private void Start()
	{
        carBehaviour = buggy.GetComponent<CarBehaviour>();
	}

	// Start is called before the first frame update
	void Update()
    {
        float currentSteeringAngle = carBehaviour.SteerAngle;
        float buggyAbsoluteRotation = buggy.transform.eulerAngles.y;

        float targetAngle = buggyAbsoluteRotation + MinAngle + 0.5f * currentSteeringAngle;

        Quaternion currentQuat = transform.rotation;
        float x = currentQuat.eulerAngles.x;
        float y = targetAngle;
        float z = currentQuat.eulerAngles.z;

        Quaternion nextQuat = Quaternion.Euler(x, y, z);
        transform.rotation = nextQuat;
    }
}
