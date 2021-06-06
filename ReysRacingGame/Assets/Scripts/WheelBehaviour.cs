using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelBehaviour : MonoBehaviour
{
    public WheelCollider wheelCol;
    public bool rotationEnabled = true;

    // Update is called once per frame
    void Update()
    {
        // Get the wheel position and rotation from the wheelcolider
		Quaternion quat; 
        Vector3 position; 
        wheelCol.GetWorldPose(out position, out quat); 
        transform.position = position;

		if (rotationEnabled)
		{
            transform.rotation = quat;
        }
        
    }
}
