using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private SFXAudio m_SFXAudioPrefab;
    [SerializeField]
    private AudioSource m_MusicPlayer;

    [SerializeField]
    private float m_VolumeToSwitchToNewMusic = 0.25f;

    private AudioClip m_NextMusicToPlay;

    private bool m_IsMusicFadingOut = false;
    private bool m_IsMusicFadingIn = false;
    private bool m_NewSceneMusic = false;

    private void Start()
    {
        LevelManager.Instance.m_OnLoadingFinished += OnSceneFinishedLoading;
        LevelManager.Instance.m_OnLoadingFinished += OnSceneStartedLoading;
        m_MusicPlayer.volume = 0;
    }

    private void Update()
    {
        if (m_IsMusicFadingIn || m_IsMusicFadingOut)
        {
            FadingInUpdate();
            FadingOutUpdate();
        }
    }

    private void FadingOutUpdate()
    {
        if (m_IsMusicFadingOut)
        {
            m_MusicPlayer.volume -= Time.deltaTime;

            if (m_NextMusicToPlay != null && m_MusicPlayer.volume <= m_VolumeToSwitchToNewMusic)
            {
                TransitioningToAnotherMusic();
            }
            else if (m_MusicPlayer.volume <= 0)
            {
                m_IsMusicFadingOut = false;
                m_MusicPlayer.Stop();
            }
        }
    }

    private void TransitioningToAnotherMusic()
    {
        m_IsMusicFadingOut = false;
        PlayMusicPlayer();
    }

    private void PlayMusicPlayer()
    {
        m_MusicPlayer.clip = m_NextMusicToPlay;
        m_NextMusicToPlay = null;
        m_IsMusicFadingIn = true;
        m_MusicPlayer.Play();
    }

    private void FadingInUpdate()
    {
        if (m_IsMusicFadingIn)
        {
            m_MusicPlayer.volume += Time.deltaTime;

            if (m_MusicPlayer.volume >= 1)
            {
                m_IsMusicFadingIn = false;
            }
        }
    }

    #region SFX FONCTION 

    public void PlaySFX(AudioClip a_Clip, Vector3 a_Position)
    {
        SFXAudio audio = Instantiate(m_SFXAudioPrefab, a_Position, Quaternion.identity);
        audio.SetupAudio(a_Clip, 1, 1, false, 0);
        audio.PlayAudio(0);
    }

    public void PlaySFX(AudioClip a_Clip, Vector3 a_Position, ulong a_Delay)
    {
        SFXAudio audio = Instantiate(m_SFXAudioPrefab, a_Position, Quaternion.identity);
        audio.SetupAudio(a_Clip, 1, 1, false, 0);
        audio.PlayAudio(a_Delay);
    }

    public void PlaySFX(AudioClip a_Clip, Vector3 a_Position, float a_Volume, float a_Pitch, bool a_FadeIn, bool a_DontDestoyOnLoad = false)
    {
        SFXAudio audio = Instantiate(m_SFXAudioPrefab, a_Position, Quaternion.identity);
        audio.SetupAudio(a_Clip, a_Volume, a_Pitch, a_FadeIn, 0, a_DontDestoyOnLoad);
        audio.PlayAudio(0);
    }

    public void PlaySFX(AudioClip a_Clip, Vector3 a_Position, float a_Volume, float a_Pitch, bool a_FadeIn, ulong a_Delay)
    {
        SFXAudio audio = Instantiate(m_SFXAudioPrefab, a_Position, Quaternion.identity);
        audio.SetupAudio(a_Clip, a_Volume, a_Pitch, a_FadeIn, 0);
        audio.PlayAudio(a_Delay);
    }

    #endregion

    #region MUSIC FONCTION

    public void PlayMusic(AudioClip a_Music)
    {
        if (a_Music != null)
        {
            m_NewSceneMusic = true;
            m_NextMusicToPlay = a_Music;
            StopMusic();
        }
    }

    public void StopMusic()
    {
        m_IsMusicFadingOut = true;
    }

    public void PauseMusic()
    {
        m_MusicPlayer.Pause();
    }

    private void OnSceneFinishedLoading()
    {
        if (!m_NewSceneMusic)
        {
            StopMusic();
        }
    }

    private void OnSceneStartedLoading()
    {
        m_NewSceneMusic = false;
    }

    #endregion
}