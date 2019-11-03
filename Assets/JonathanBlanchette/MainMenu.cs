using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenu : MonoBehaviour
{
    
    public void SinglePlayer()
    {
        GameManager.Instance.PlayerCount = 1;       
        LevelManager.Instance.ChangeLevel("SelectionMenu",true,-1);
    }

    public void MultiPlayer()
    {
        GameManager.Instance.PlayerCount = 2;
        LevelManager.Instance.ChangeLevel("SelectionMenu",true,-1);
    }

    public void ExitGame()
    {
        LevelManager.Instance.ChangeLevel("SelectionMenu",true,-1);
    }
}
