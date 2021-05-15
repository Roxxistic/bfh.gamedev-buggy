using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class UnderWaterBehaviour : MonoBehaviour
{
    public float waterAltitude;

    private PostProcessLayer layer;

    // Start is called before the first frame update
    void Start()
    {
        layer = GetComponent<PostProcessLayer>();
    }

    // Update is called once per frame
    void Update()
    {
        layer.enabled = transform.position.y < waterAltitude;
	}
}
