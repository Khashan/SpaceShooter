using UnityEngine;

public class AppLauncher : MonoBehaviour
{
    [SerializeField]
    private string m_LevelToLoad = "MainMenu";

    private void Start()
    {
        LevelManager.Instance.ChangeLevel(m_LevelToLoad, false, 0f);
    }
}
