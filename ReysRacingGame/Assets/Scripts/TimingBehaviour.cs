using System.Collections;
using UnityEngine;

public class TimingBehaviour : MonoBehaviour
{
    public int countMax = 3;
    private CarBehaviour _carScript;
    
    public int CountDown { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        _carScript = GameObject.Find("buggy").GetComponent<CarBehaviour>();
        _carScript.thrustEnabled = false;

        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
	{
        for (CountDown = countMax; CountDown > 0; CountDown--)
		{
            yield return new WaitForSeconds(1);
		}

        _carScript.thrustEnabled = true;
    }
}
