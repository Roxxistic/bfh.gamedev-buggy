using System.Collections;
using UnityEngine;

public class TimingCountdownBehaviour : MonoBehaviour
{
    public int countMax = 3;
    
    public int CountDown { get; private set; }
    public bool IsCountdownDone => CountDown <= 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
	{
        for (CountDown = countMax; CountDown > 0; CountDown--)
		{
            yield return new WaitForSeconds(1);
		}
    }
}
