using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_ScoreText;
    [SerializeField]
    private GameObject m_GameOverGroup;
    [SerializeField]
    private GameObject m_BossDiedGroup;
    [SerializeField]
    private string m_MainMenuSceneName = "MainMenu";

    private bool m_GameOver;

    private void Awake()
    {
        GameManager.Instance.GameUI = this;
    }

    private void Start()
    {
        GameManager.Instance.RestartStats();
        m_GameOver = false;
        UpdateScore(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    public void BossDied()
    {
        m_BossDiedGroup.SetActive(true);
        Time.timeScale = 0;
    }

    public void KeepPlaying()
    {
        m_BossDiedGroup.SetActive(false);
        Time.timeScale = 1;
    }

    public void UpdateScore(int aScore)
    {
        m_ScoreText.text = "Score: " + aScore;
    }

    public void GameOver()
    {
        m_GameOver = true;
        m_GameOverGroup.SetActive(true);
    }

    public void RestartLevel()
    {
        LevelManager.Instance.ChangeLevel(gameObject.scene.name, false);
    }

    public void ReturnToMainMenu()
    {
        LevelManager.Instance.ChangeLevel(m_MainMenuSceneName, false);
        Time.timeScale = 1;
    }
}