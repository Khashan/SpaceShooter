using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_SlotOne;
    [SerializeField]
    private GameObject m_SlotTwo;
    [SerializeField]
    private GameObject m_SlotThree;

    private int m_Players;


    private void Start()
    {
        m_Players = GameManager.Instance.PlayerCount;
        Debug.Log("Nombre de joueurs" + m_Players);
    }
    
    private void Update()
    {

        if(Input.GetKey(KeyCode.KeypadEnter))
        {
            LevelManager.Instance.ChangeLevel("Done_Main",true,-1);
        }
    }
}
