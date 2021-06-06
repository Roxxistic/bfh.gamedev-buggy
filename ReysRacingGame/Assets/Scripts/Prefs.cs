using UnityEngine;

public class Prefs
{
	public readonly string suspensionDistanceKey = "suspensionDistance";
	public readonly string suspensionSpringKey = "suspensionSpring";
	public readonly string suspensionDamperKey = "suspensionDamper";
	public readonly string wheelFrictionForewardKey = "wheelFrictionForeward";
	public readonly string wheelFrictionSidewaysKey = "wheelFrictionSideways";
	public readonly string bodyColorHueKey = "bodyColorHue";
	public readonly string bodyColorSaturationKey = "bodyColorSaturation";
	public readonly string bodyColorLuminanceKey = "bodyColorLuminance";
	public readonly string bodyFeatureFlagsKey = "bodyFeatureFlags";

	public readonly float suspensionDistanceDefault = 0.2f;
	public readonly float suspensionSpringDefault = 33674f;
	public readonly float suspensionDamperDefault = 5306f;
	public readonly float wheelFrictionForewardDefault = 5.0f;
	public readonly float wheelFrictionSidewaysDefault = 2.1f;
	public readonly float bodyColorHueDefault = 1.0f;
	public readonly float bodyColorSaturationDefault = 1.0f;
	public readonly float bodyColorLuminanceDefault = 1.0f;
	public readonly bool bodyFeatureFlagsDefault = true;

	public float suspensionDistance;
	public float suspensionSpring;
	public float suspensionDamper;
	public float wheelFrictionForeward;
	public float wheelFrictionSideways;
	public float bodyColorHue;
	public float bodyColorSaturation;
	public float bodyColorLuminance;
	public bool bodyFeatureFlags;

	public void Load()
	{
		suspensionDistance = PlayerPrefs.GetFloat(suspensionDistanceKey, suspensionDistanceDefault);
		suspensionSpring = PlayerPrefs.GetFloat(suspensionSpringKey, suspensionSpringDefault);
		suspensionDamper = PlayerPrefs.GetFloat(suspensionDamperKey, suspensionDamperDefault);
		wheelFrictionForeward = PlayerPrefs.GetFloat(wheelFrictionForewardKey, wheelFrictionForewardDefault);
		wheelFrictionSideways = PlayerPrefs.GetFloat(wheelFrictionSidewaysKey, wheelFrictionSidewaysDefault);
		bodyColorHue = PlayerPrefs.GetFloat(bodyColorHueKey, bodyColorHueDefault);
		bodyColorSaturation = PlayerPrefs.GetFloat(bodyColorSaturationKey, bodyColorSaturationDefault);
		bodyColorLuminance = PlayerPrefs.GetFloat(bodyColorLuminanceKey, bodyColorLuminanceDefault);
		bodyFeatureFlags = PlayerPrefs.GetInt(bodyFeatureFlagsKey) == 1;
	}

	public void Save()
	{
		PlayerPrefs.SetFloat(suspensionDistanceKey, suspensionDistance);
		PlayerPrefs.SetFloat(suspensionSpringKey, suspensionSpring);
		PlayerPrefs.SetFloat(suspensionDamperKey, suspensionDamper);
		PlayerPrefs.SetFloat(wheelFrictionForewardKey, wheelFrictionForeward);
		PlayerPrefs.SetFloat(wheelFrictionSidewaysKey, wheelFrictionSideways);
		PlayerPrefs.SetFloat(bodyColorHueKey, bodyColorHue);
		PlayerPrefs.SetFloat(bodyColorSaturationKey, bodyColorSaturation);
		PlayerPrefs.SetFloat(bodyColorLuminanceKey, bodyColorLuminance);
		PlayerPrefs.SetInt(bodyFeatureFlagsKey, bodyFeatureFlags ? 1 : 0);
	}

	public void SetAll(ref GameObject buggy, ref WheelCollider wheelFL, ref WheelCollider wheelFR, ref WheelCollider wheelRL, ref WheelCollider wheelRR, ref GameObject flagLeft, ref GameObject flagRight)
	{
		SetWheelColliderSuspension(ref wheelFL, ref wheelFR, ref wheelRL, ref wheelRR);
		SetBodyColor(ref buggy);
		SetFeatureFlag(ref flagLeft, ref flagRight);
	}

	public void SetWheelColliderSuspension(ref WheelCollider wheelFL, ref WheelCollider wheelFR, ref WheelCollider wheelRL, ref WheelCollider wheelRR)
	{
		wheelFL.suspensionDistance = suspensionDistance;
		wheelFR.suspensionDistance = suspensionDistance;
		wheelRL.suspensionDistance = suspensionDistance;
		wheelRR.suspensionDistance = suspensionDistance;

		var joint = new JointSpring()
		{
			spring = suspensionSpring,
			damper = suspensionDamper
		};

		wheelFL.suspensionSpring = joint;
		wheelFR.suspensionSpring = joint;
		wheelRL.suspensionSpring = joint;
		wheelRR.suspensionSpring = joint;

		WheelFrictionCurve f_fwWFC = wheelFL.forwardFriction;
		WheelFrictionCurve f_swWFC = wheelFL.sidewaysFriction;

		f_fwWFC.stiffness = wheelFrictionForeward;
		f_swWFC.stiffness = wheelFrictionSideways;

		wheelFL.forwardFriction = f_fwWFC;
		wheelFL.sidewaysFriction = f_swWFC;
		wheelFR.forwardFriction = f_fwWFC;
		wheelFR.sidewaysFriction = f_swWFC;

		wheelRL.forwardFriction = f_fwWFC;
		wheelRL.sidewaysFriction = f_swWFC;
		wheelRR.forwardFriction = f_fwWFC;
		wheelRR.sidewaysFriction = f_swWFC;
	}

	public void SetBodyColor(ref GameObject buggy) {
		var renderer = buggy.GetComponent<Renderer>();
		var color = Color.HSVToRGB(bodyColorHue, bodyColorSaturation, bodyColorLuminance);
		renderer.material.color = color;
	}

	public void SetFeatureFlag(ref GameObject flagLeft, ref GameObject flagRight)
	{
		flagLeft.SetActive(bodyFeatureFlags);
		flagRight.SetActive(bodyFeatureFlags);
	}
}
