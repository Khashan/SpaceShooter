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
    private string m_MainMenuSceneName = "MainMenu";

    private bool m_GameOver;
    private int m_Score;

    private void Awake()
    {
        GameManager.Instance.GameUI = this;
    }

    private void Start()
    {
        GameManager.Instance.RestartStats();
        m_GameOver = false;
        m_Score = 0;
        UpdateScore(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    public void UpdateScore(int aScore)
    {
        m_ScoreText.text = "Score: " + m_Score;
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
    }
}