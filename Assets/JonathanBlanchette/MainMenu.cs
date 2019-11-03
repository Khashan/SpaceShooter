using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartButton()
    {
        LevelManager.Instance.ChangeLevel("SelectionMenu",true,-1);
    }
}
