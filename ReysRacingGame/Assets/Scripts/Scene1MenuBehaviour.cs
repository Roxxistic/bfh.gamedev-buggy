using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene1MenuBehaviour : MonoBehaviour
{
	private Prefs _prefs;

	public WheelCollider wheelColliderFL;
	public WheelCollider wheelColliderFR;
	public WheelCollider wheelColliderRL;
	public WheelCollider wheelColliderRR;

	public GameObject buggy;
	public GameObject FlagLeft;
	public GameObject FlagRight;

	void Start()
	{
		_prefs = new Prefs();
		_prefs.Load();
		_prefs.SetAll(ref buggy, ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR, ref FlagLeft, ref FlagRight);
	}

	public void OnMenuClick()
	{
		SceneManager.LoadScene("SceneMenu");
	}
}
