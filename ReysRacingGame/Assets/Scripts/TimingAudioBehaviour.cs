using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingAudioBehaviour : MonoBehaviour
{
    public AudioClip countdownSoundClip;
    public TimingCountdownBehaviour timingBehaviourScript;

    private AudioSource _audioSource;
    private int _previousCountdown = -1;
    private static float _defaultPitch = 1.0f;
    private static float _countdownZeroPitch = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        // Configure AudioSource component by program
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = countdownSoundClip;
        _audioSource.loop = false;
        _audioSource.volume = 1.0f;
        _audioSource.playOnAwake = false;
        _audioSource.enabled = false; // Bugfix
        _audioSource.enabled = true; // Bugfix


        _previousCountdown = timingBehaviourScript.countMax + 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetCountdownSound(timingBehaviourScript.CountDown);
    }

    void SetCountdownSound(int countdown)
    {
        if (_audioSource == null) return;

        // Only beep once when zero.
        if (countdown == 0 && _previousCountdown == 0) return;

        _audioSource.pitch = countdown > 0 ? _defaultPitch : _countdownZeroPitch;

        if (_previousCountdown > countdown)
		{
            Debug.Log("countdown decreased");
            _audioSource.Play();
            _previousCountdown--;
        }
    }
}
