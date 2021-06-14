using System.Collections;
using System;
using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    public int ChoiceInterval = 3;
    public Transform[] targets;
    public float movementSpeed = 0.01f;
    public float stoppingDistance = 0.1f;
    public float turnSpeed = 2f;
    public float turnAwayFromCarTurnSpeed = 3f;
    public float turnAwayFromCarMovementSpeed = 0.1f;
    public float minDistanceToCar = 10f;

    public Transform car;
    public Terrain terrain;

    private Vector3 targetPosition;
    private System.Random random = new System.Random();


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeDirection());
    }

    // Update is called once per frame
    void Update()
    {
        var currentDistanceToCar = Vector3.Distance(car.position, transform.position);

        if(currentDistanceToCar <= minDistanceToCar)
		{
            SwimAwayFromCar();
		} else
		{
            SwimToTarget();
		}
    }

    private void SwimAwayFromCar() {

        var direction = transform.position - new Vector3(car.position.x, transform.position.y, car.position.z);

        var lookRotation = Quaternion.LookRotation(direction);
        var approximateLookRotation = Quaternion.Lerp(transform.rotation, lookRotation, turnAwayFromCarTurnSpeed * Time.deltaTime);

        transform.rotation = approximateLookRotation;

        transform.position += (direction).normalized * turnAwayFromCarMovementSpeed;
    }


    private void SwimToTarget()
	{
        var distance = Vector3.Distance(targetPosition, transform.position);

        if (distance > stoppingDistance)
        {
            var direction = new Vector3(targetPosition.x, transform.position.y, targetPosition.z) - transform.position;
            var lookRotation = Quaternion.LookRotation(direction);
            var approximateLookRotation = Quaternion.Lerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);

            transform.rotation = approximateLookRotation;

            transform.position += (direction).normalized * movementSpeed;
        }
    }


    IEnumerator ChangeDirection()
    {
		while (true)
		{
            var next = random.Next(0, targets.Length);
            targetPosition = targets[next].position;

            yield return new WaitForSeconds(ChoiceInterval);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < targets.Length; i++)
        {
            if (i < targets.Length - 1)
            {
                Gizmos.DrawLine(targets[i].position, targets[i + 1].position);
            }
            else
            {
                Gizmos.DrawLine(targets[i].position, targets[0].position);
            }
        }
    }
}
