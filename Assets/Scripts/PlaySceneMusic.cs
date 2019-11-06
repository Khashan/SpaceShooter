using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneMusic : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_SceneMusic;

    private void Awake()
    {
        AudioManager.Instance.PlayMusic(m_SceneMusic);
    }
}
