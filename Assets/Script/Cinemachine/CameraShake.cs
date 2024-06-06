using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//This script is attached to Cmvcam1 gameObject
public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;

    private void Awake()
    {
        instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeChamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMutiChannelPerlin =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMutiChannelPerlin.m_AmplitudeGain = intensity;

        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {

                CinemachineBasicMultiChannelPerlin cinemachineBasicMutiChannelPerlin =
                        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMutiChannelPerlin.m_AmplitudeGain = 0f;

            }
        }
    }
}
