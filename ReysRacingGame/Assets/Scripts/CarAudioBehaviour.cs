using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudioBehaviour : MonoBehaviour
{
    public AudioClip engineSingleRPMSoundClip;
    private AudioSource _engineAudioSource;

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
    void Update()
    {
        
    }
}
