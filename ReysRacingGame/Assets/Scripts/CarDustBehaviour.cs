using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class CarDustBehaviour : MonoBehaviour
	{

		public CarBehaviour engine;
		private ParticleSystem _ps;
		private ParticleSystem.EmissionModule _dustEmission;

		// Start is called before the first frame update
		void Start()
		{
			_ps = GetComponent<ParticleSystem>();
			_dustEmission = _ps.emission;
			_dustEmission.enabled = true;
		}

		// Update is called once per frame
		void FixedUpdate()
		{
			SetDustRate(engine.CurrentSpeedKMH);
		}

		void SetDustRate(float speedKmh)
		{
			float dustRate = 0;
			if(speedKmh > 10.0f && engine._carIsOnDrySand)
			{
				dustRate = speedKmh;
			}
			_dustEmission.rateOverDistance = new ParticleSystem.MinMaxCurve(dustRate);
		}
	}
}