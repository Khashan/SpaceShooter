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
}
