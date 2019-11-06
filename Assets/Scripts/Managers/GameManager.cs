using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
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
}
