using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int m_PlayerCount;
    public int PlayerCount
    {
        get {return m_PlayerCount;}  
        set { m_PlayerCount = value; }   
    }

    private List<GameObject> m_ListShipSelect = new List<GameObject>();
    public List<GameObject> ListShipSelect
    {
        get { return m_ListShipSelect; }
        set { m_ListShipSelect = value; }
    }
}
