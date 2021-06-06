using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFixer: MonoBehaviour
{
    // Start is called before the first frame update
    void LateUpdate() { 
        transform.position = new Vector3(0.0f, transform.position.y, 0.0f); 
    }
}

public class WheelFreezer : MonoBehaviour
{
    // Start is called before the first frame update
    void LateUpdate()
    {
        transform.position = new Vector3(0.0f, transform.position.y, 0.0f);
    }
}
