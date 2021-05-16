using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class CarExhaustBehaviour : MonoBehaviour
	{

		public CarBehaviour engine;

		private ParticleSystem.EmissionModule _smokeEmission;
		private ParticleSystem _ps;

		// Start is called before the first frame update
		void Start()
		{
			_ps = GetComponent<ParticleSystem>();
			_smokeEmission = _ps.emission;
			_smokeEmission.enabled = true;
		}

		// Update is called once per frame
		void FixedUpdate()
		{
			SetSmokeRate(engine.CurrentSpeedRPM);
		}

		void SetSmokeRate(float engineRPM)
		{
			float smokeRate = engineRPM / 50.0f;
			_smokeEmission.rateOverDistance = new ParticleSystem.MinMaxCurve(smokeRate);
		}
	}
}