using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXAudio : MonoBehaviour
{

    [SerializeField]
    private AudioSource m_AudioSource;

    private float m_CurrentTime = 0f;
    private float m_Duration;

    //Fade Setting
    private bool m_FadeIn = false;
    private float m_MaxVolume = 0f;
    private float m_FadeValue = 0f;

    public void SetupAudio(AudioClip a_Clip, float a_Volume, float a_Pitch, bool a_FadeIn, float a_3DEffect, bool a_DontDestoyOnLoad = false)
    {
        m_AudioSource.clip = a_Clip;
        m_AudioSource.pitch = a_Pitch;
        m_AudioSource.spatialBlend = a_3DEffect;
        m_Duration = a_Clip.length;
        if (!a_FadeIn)
        {
            m_AudioSource.volume = a_Volume;
        }
        else
        {
            m_FadeIn = a_FadeIn;
            m_AudioSource.volume = 0;
            m_MaxVolume = a_Volume;
            m_FadeValue = a_Volume * 0.02f;
        }

        if (a_DontDestoyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayAudio(ulong a_Delay)
    {
        m_AudioSource.Play(a_Delay);
    }

    private void Update()
    {
        m_CurrentTime += Time.deltaTime;
        if (m_CurrentTime >= m_Duration)
        {
            Destroy(gameObject);
        }

        if (m_FadeIn && m_AudioSource.volume < m_MaxVolume)
        {
            m_AudioSource.volume += m_FadeValue;
        }
    }
}