using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Child Object")]
    [SerializeField] private GameObject menuUIGroup;
    private bool isPaused = false;

    private void Start()
    {
        Debug.Log("PauseMenuManager Start() 실행됨");
        menuUIGroup.SetActive(false); // 시작 시 꺼놓기     
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        Debug.Log("PauseGame() 호출됨");
        isPaused = true;
        foreach (Rigidbody2D rb in FindObjectsOfType<Rigidbody2D>())
            rb.simulated = false;
        menuUIGroup.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        foreach (Rigidbody2D rb in FindObjectsOfType<Rigidbody2D>())
            rb.simulated = true;
        menuUIGroup.SetActive(false);
        Time.timeScale = 1f;
    }    

    public void QuitToMain()
    {
        Time.timeScale = 1f;

        // 1. 전역 오브젝트 전부 제거
        Managers.ResetAllDontDestroyOnLoad();
        SceneManager.LoadScene("MainScene");
    }    
}
