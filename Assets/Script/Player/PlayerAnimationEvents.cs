using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This scipt is attached to Bubble_GFX

public class PlayerAnimationEvents : MonoBehaviour
{
    private Player_Movement _pm;
    private ParticleSystem footStepsParticle;

    private void Start()
    {
        _pm = GetComponentInParent<Player_Movement>();
    }

    public void PlayLeftFootstepAudio()
    {
        FindObjectOfType<Audio_Manager>().Play("LeftFootsteps");
    }
    public void PlayRightFootstepAudio()
    {
        FindObjectOfType<Audio_Manager>().Play("RightFootsteps");

    }

    public void FootStepsParticalEffect()
    {   
        _pm.footStepsEffect().Play();
    }

    public void StopFootStepsParticalEffect()
    {
        _pm.footStepsEffect().Stop();
    }
}

