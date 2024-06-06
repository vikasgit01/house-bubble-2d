using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

//This script is attached to Audio Manager GameObject

public class Audio_Manager : MonoBehaviour
{
    [SerializeField] private Sound[] sound;

    private void Awake()
    {
        foreach (Sound s in sound)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();

            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
         }
    }

    private void Start()
    {
        Play("Wind");
        Play("Music");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sound, sound => sound.audioName == name);
        if (s == null) return;
        s.audioSource.Play();
    }
}
