using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to TimerAnimation Game object
public class Clock_Sound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clockClip;
    [SerializeField] private AudioClip clockCompleteClip;

    public void PlayAudio()
    {
        if (Spawn_Manager.instance._currentPhaseTime > 1)
        {
            FindObjectOfType<Audio_Manager>().Play("ClockStart");
        }
        else
        {
            FindObjectOfType<Audio_Manager>().Play("ClockComplete");
        }

    }
}
