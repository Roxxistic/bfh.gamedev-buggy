using UnityEngine;

public class RescueBehaviour : MonoBehaviour
{
    public Transform[] spots = { };

    public bool DoRescue => Input.GetKey("r");

    private CarBehaviour car;

	private void Start()
	{
        car = GetComponent<CarBehaviour>();
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		if (DoRescue)
		{
            var closestSpot = FindClosestRescueSpot();

            transform.position = closestSpot.position;

            car.FreezeAfterRescue();
            //car.torque = 0;
            Input.ResetInputAxes();

            
		}
    }


    private Transform FindClosestRescueSpot()
	{
        if(spots.Length <= 0)
		{
            return transform;
		}

        Transform closest = null;
        float distance = Mathf.Infinity;
        Vector3 buggyPosition = transform.position;

        foreach(Transform spot in spots)
		{
            Vector3 diff = spot.position - buggyPosition;
            float curDistance = diff.sqrMagnitude;
            if(curDistance < distance)
			{
                closest = spot;
                distance = curDistance;
			}
		}

        return closest;
	}

}
