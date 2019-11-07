using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private GameUI m_UI;
    public GameUI GameUI
    {
        get { return m_UI; }
        set { m_UI = value; }
    }

    private int m_Scores;

    private int m_PlayerCount;
    public int PlayerCount
    {
        get { return m_PlayerCount; }
        set { m_PlayerCount = value; }
    }

    private bool m_BossDied = false;
    public bool BossDead
    {
        get { return m_BossDied; }
        set { m_BossDied = value; }
    }

    private int m_DeadPlayerCount = 0;

    private List<GameObject> m_ListShipSelect = new List<GameObject>();
    public List<GameObject> ListShipSelect
    {
        get { return m_ListShipSelect; }
        set { m_ListShipSelect = value; }
    }

    private GameObject m_ShipPlayerOne;
    public GameObject ShipPlayerOne
    {
        get { return m_ShipPlayerOne; }
        set { m_ShipPlayerOne = value; }
    }

    private GameObject m_ShipPlayerTwo;
    public GameObject ShipPlayerTwo
    {
        get { return m_ShipPlayerTwo; }
        set { m_ShipPlayerTwo = value; }
    }

    public void PlayerDeath()
    {
        m_DeadPlayerCount++;

        if (m_DeadPlayerCount == m_PlayerCount)
        {
            m_UI.GameOver();
        }
    }

    public void RestartStats()
    {
        m_DeadPlayerCount = 0;
        m_Scores = 0;
    }

    public void AddScore(int aScore)
    {
        m_Scores += aScore;
    }
}
