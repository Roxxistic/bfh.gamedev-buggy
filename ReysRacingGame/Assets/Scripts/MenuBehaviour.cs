using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour
{
	public WheelCollider wheelColliderFL;
	public WheelCollider wheelColliderFR;
	public WheelCollider wheelColliderRL;
	public WheelCollider wheelColliderRR;
	public Slider sliderSuspensionDistance;
	public Text textDistanceNumber;

	private Prefs _prefs;

	void Start()
	{
		_prefs = new Prefs();
		_prefs.Load();
		_prefs.SetAll(ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR);
		sliderSuspensionDistance.value = _prefs.suspensionDistance;
		textDistanceNumber.text = sliderSuspensionDistance.value.ToString("0.00");
	}

	public void OnSliderChangedSuspensionDistance(float nextValue)
	{
		textDistanceNumber.text = sliderSuspensionDistance.value.ToString("0.00");
		_prefs.suspensionDistance = sliderSuspensionDistance.value;
		_prefs.SetWheelColliderSuspension(ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR);
	}

	public void OnStartClick()
	{
		_prefs.Save();
		SceneManager.LoadScene("Scene1");
	}

	private void OnApplicationQuit()
	{
		_prefs.Save();
	}
}
