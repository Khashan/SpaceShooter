using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Done_GameController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_ScoreText;

    [SerializeField]
    private TextMeshProUGUI m_RestartText;

    [SerializeField]
    private Image m_GameOverImage;

    private bool m_GameOver;
    private bool m_Restart;
    private int m_Score;

    private void Start()
    {
        m_GameOver = false;
        m_Restart = false;
        m_RestartText.text = "";
        //m_GameOverText.text = "";
        m_Score = 0;
        UpdateScore();
    }

    private void Update()
    {
        if (m_Restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                LevelManager.Instance.ChangeLevel("Game",false);
            }
        }
    }

    public void AddScore(int newScoreValue)
    {
        m_Score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        m_ScoreText.text = "Score: " +m_Score;
    }

    public void GameOver()
    {
        //m_GameOverText.text = "Game Over!";
        m_GameOver = true;
        m_GameOverImage.enabled = true;
    }

    private void OnTriggerEnter(Collider aCol)
    {
        if(aCol.tag == "Destroy")
        {
            Destroy(gameObject);
            Destroy(this);
        }
    }
}