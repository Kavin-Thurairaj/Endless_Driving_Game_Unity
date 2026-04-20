using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManagerScript : MonoBehaviour
{

    public static GameManagerScript Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps object alive across scenes
        }
        else
        {
            Destroy(gameObject); // Prevents duplicates when returning to scenes
        }
    }
    

    public void OnMainMenuClick()
    {
        Debug.Log("Main Menu Clicked");
        SceneManager.LoadScene("Main Menu");
    }

    public void OnExitGameClicked()
    {
        Debug.Log("Quit Game");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops Play Mode in Editor
        #endif
    }
}
