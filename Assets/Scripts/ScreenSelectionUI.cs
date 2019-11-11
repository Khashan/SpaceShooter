using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSelectionUI : MonoBehaviour
{
    [SerializeField]
    private string m_MainMenuName = "MainMenu";

    public void GoBackToMainMenu()
    {
        LevelManager.Instance.ChangeLevel(m_MainMenuName, true, -1);
    }

}
