using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudioBehaviour : MonoBehaviour
{
    public AudioClip engineSingleRPMSoundClip;
    public CarBehaviour carBehaviour;

    private AudioSource _engineAudioSource;

    private static float _minRPM = 800;
    private static float _maxRPM = 8000;
    private static float _minPitch = 0.3f;
    private static float _maxPitch = 3.0f;
    private readonly float _slope = (_maxPitch - _minPitch) / (_maxRPM - _minRPM);

    // Start is called before the first frame update
    void Start()
    {
        // Configure AudioSource component by program
        _engineAudioSource = gameObject.AddComponent<AudioSource>();
        _engineAudioSource.clip = engineSingleRPMSoundClip;
        _engineAudioSource.loop = true;
        _engineAudioSource.volume = 0.7f;
        _engineAudioSource.playOnAwake = true;
        _engineAudioSource.enabled = false; // Bugfix
        _engineAudioSource.enabled = true; // Bugfix
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetEngineSound(carBehaviour.CurrentSpeedRPM);
    }

    void SetEngineSound(float engineRpm)
	{
        if (_engineAudioSource == null) return;

        float pitch = _minPitch + _slope * engineRpm;
        _engineAudioSource.pitch = pitch;
    }
}
