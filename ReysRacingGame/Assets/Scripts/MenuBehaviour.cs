using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour
{
	public WheelCollider wheelColliderFL;
	public WheelCollider wheelColliderFR;
	public WheelCollider wheelColliderRL;
	public WheelCollider wheelColliderRR;

	public GameObject buggy;
	public GameObject FlagLeft;
	public GameObject FlagRight;

	public Slider sliderSuspensionDistance;
	public Slider sliderSuspensionSpring;
	public Slider sliderSuspensionDamper;
	public Slider sliderWheelFrictionForeward;
	public Slider sliderWheelFrictionSideways;
	public Slider sliderBodyColorHue;
	public Slider sliderBodyColorSaturation;
	public Slider sliderBodyColorLuminance;
	public Toggle toggleBodyFeatureFlag;

	public Text textDistanceNumber;
	public Text textSuspensionSpringNumber;
	public Text textSuspensionDamperNumber;
	public Text textWheelFrictionForewardNumber;
	public Text textWheelFrictionSidewaysNumber;
	public Text textBodyColorHueNumber;
	public Text textBodyColorSaturationNumber;
	public Text textBodyColorLuminanceNumber;

	private Prefs _prefs;

	void Start()
	{
		_prefs = new Prefs();
		_prefs.Load();
		_prefs.SetAll(ref buggy, ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR, ref FlagLeft, ref FlagRight);
		sliderSuspensionDistance.value = _prefs.suspensionDistance;
		sliderSuspensionSpring.value = _prefs.suspensionSpring;
		sliderSuspensionDamper.value = _prefs.suspensionDamper;
		sliderWheelFrictionForeward.value = _prefs.wheelFrictionForeward;
		sliderWheelFrictionSideways.value = _prefs.wheelFrictionSideways;
		sliderBodyColorHue.value = _prefs.bodyColorHue;
		sliderBodyColorSaturation.value = _prefs.bodyColorSaturation;
		sliderBodyColorLuminance.value = _prefs.bodyColorLuminance;
		textDistanceNumber.text = sliderSuspensionDistance.value.ToString("0.00");
		textSuspensionSpringNumber.text = sliderSuspensionSpring.value.ToString("0.00");
		textSuspensionDamperNumber.text = sliderSuspensionDamper.value.ToString("0.00");
		textWheelFrictionForewardNumber.text = sliderWheelFrictionForeward.value.ToString("0.00");
		textWheelFrictionSidewaysNumber.text = sliderWheelFrictionSideways.value.ToString("0.00");
		textBodyColorHueNumber.text = sliderBodyColorHue.value.ToString("0.00");
		textBodyColorSaturationNumber.text = sliderBodyColorSaturation.value.ToString("0.00");
		textBodyColorLuminanceNumber.text = sliderBodyColorLuminance.value.ToString("0.00");
	}

	public void OnSliderChangedSuspensionDistance(float nextValue)
	{
		textDistanceNumber.text = sliderSuspensionDistance.value.ToString("0.00");
		_prefs.suspensionDistance = sliderSuspensionDistance.value;
		_prefs.SetWheelColliderSuspension(ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR);
	}

	public void OnSliderChangedSuspensionSpring(float nextValue)
	{
		textSuspensionSpringNumber.text = sliderSuspensionSpring.value.ToString("0.00");
		_prefs.suspensionSpring = sliderSuspensionSpring.value;
		_prefs.SetWheelColliderSuspension(ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR);
	}

	public void OnSliderChangedSuspensionDamper(float nextValue)
	{
		textSuspensionDamperNumber.text = sliderSuspensionDamper.value.ToString("0.00");
		_prefs.suspensionDamper = sliderSuspensionDamper.value;
		_prefs.SetWheelColliderSuspension(ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR);
	}

	public void OnSliderChangedWheelFrictionForeward(float nextValue)
	{
		textWheelFrictionForewardNumber.text = sliderWheelFrictionForeward.value.ToString("0.00");
		_prefs.wheelFrictionForeward = sliderWheelFrictionForeward.value;
		_prefs.SetWheelColliderSuspension(ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR);
	}

	public void OnSliderChangedWheelFrictionSideways(float nextValue)
	{
		textWheelFrictionSidewaysNumber.text = sliderWheelFrictionSideways.value.ToString("0.00");
		_prefs.wheelFrictionSideways = sliderWheelFrictionSideways.value;
		_prefs.SetWheelColliderSuspension(ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR);
	}

	public void OnSliderChangedBodyColorHue(float nextValue)
	{
		textBodyColorHueNumber.text = sliderBodyColorHue.value.ToString("0.00");
		_prefs.bodyColorHue = sliderBodyColorHue.value;
		_prefs.SetBodyColor(ref buggy);
	}

	public void OnSliderChangedBodyColorSaturation(float nextValue)
	{
		textBodyColorSaturationNumber.text = sliderBodyColorSaturation.value.ToString("0.00");
		_prefs.bodyColorSaturation = sliderBodyColorSaturation.value;
		_prefs.SetBodyColor(ref buggy);
	}

	public void OnSliderChangedBodyColorLuminance(float nextValue)
	{
		textBodyColorLuminanceNumber.text = sliderBodyColorLuminance.value.ToString("0.00");
		_prefs.bodyColorLuminance = sliderBodyColorLuminance.value;
		_prefs.SetBodyColor(ref buggy);
	}

	public void OnToggleBodyFeatureFlag(bool nextValue)
	{
		_prefs.bodyFeatureFlags = toggleBodyFeatureFlag.isOn;
		_prefs.SetFeatureFlag(ref FlagLeft, ref FlagRight);
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
