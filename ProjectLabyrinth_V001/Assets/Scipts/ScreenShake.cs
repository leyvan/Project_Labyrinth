using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private CinemachineFreeLook cam;
    private float startingIntensity;
    private float fadeOutTimer;
    private float fadeTime;
    private bool fadeOut;

    void Awake()
    {
        cam = GetComponent<CinemachineFreeLook>();

    }
    public void ShakeEffect(float intensity, bool fade = false)
    {
        CinemachineBasicMultiChannelPerlin effectComp = cam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

        effectComp.m_AmplitudeGain = intensity;

        startingIntensity = intensity;
        fadeOut = fade;
    }
}
