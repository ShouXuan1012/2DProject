using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("TutorialScene");        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
