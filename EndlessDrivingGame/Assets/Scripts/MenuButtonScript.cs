using UnityEngine;

public class MenuButtonScript : MonoBehaviour
{
    public void QuitGame()
    {
        GameManagerScript.Instance.OnExitGameClicked();
    }

    public void MainMenuClicked()
    {
        GameManagerScript.Instance.OnMainMenuClick();
    }
}
