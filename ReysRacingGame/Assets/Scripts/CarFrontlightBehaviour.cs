using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HorizontalDirection
{
    Left,
    Straight,
    Right
}

public enum RelativePostion
{
    Inner,
    Outer
}

public class CarFrontlightBehaviour : MonoBehaviour
{
    public WheelCollider wheelCol;
    public HorizontalDirection direction;
    public RelativePostion relativePostion;

    // Start is called before the first frame update
    void Update()
    {
		// Get the wheel position and rotation from the wheelcolider
		Quaternion quat;
		wheelCol.GetWorldPose(out _, out quat);

        int rotationApprox = (int)(quat.y * 100) % 1000;

        if(
            ((rotationApprox > 0 && direction == HorizontalDirection.Right) ||
            (rotationApprox < 0 && direction == HorizontalDirection.Left)) &&
            relativePostion == RelativePostion.Outer
            )
		{
            Quaternion currentQuat = transform.rotation;
            Quaternion nextQuat = new Quaternion(currentQuat.x, quat.y / 2, currentQuat.z, currentQuat.w);
            transform.rotation = nextQuat;
        }
    }
}
