using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuLoadSlotManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject loadSlotPanel;
    public Button[] loadSlotButtons;

    public void StartGame()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        mainPanel.SetActive(true);
        loadSlotPanel.SetActive(false);
    }

    public void OpenLoadSlots()
    {
        mainPanel.SetActive(false);
        loadSlotPanel.SetActive(true);

        for (int i = 0; i < loadSlotButtons.Length; i++)
        {
            var text = loadSlotButtons[i].GetComponentInChildren<TMP_Text>();

            if (SaveSystem.HasSave(i))
            {
                loadSlotButtons[i].interactable = true;
                text.text = $"ΩΩ∑‘ {i + 1} (¿˙¿Âµ )";
            }
            else
            {
                loadSlotButtons[i].interactable = false;
                text.text = $"ΩΩ∑‘ {i + 1} (∫Ûƒ≠)";
            }
        }
    }

    public void CloseLoadSlots()
    {
        loadSlotPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void LoadSlot(int slot)
    {
        SaveData data = SaveSystem.Load(slot);
        if (data == null)
            return;

        PlayerPrefs.SetInt("LoadSlotIndex", slot); // ∞‘¿” æ¿ø°º≠ »Æ¿Œ«œ±‚ ¿ß«ÿ
        SceneManager.LoadScene(data.currentScene);
    }
}
