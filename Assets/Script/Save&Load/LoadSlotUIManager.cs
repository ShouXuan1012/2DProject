using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSlotUIManager : MonoBehaviour
{
    [Header("Panels")]    
    public GameObject loadSlotPanel;

    [Header("슬롯 버튼들")]
    public Button[] slotButtons;

    private void Start()
    {
        loadSlotPanel.SetActive(false);        
    }

    public void OpenLoadSlots()
    {        
        loadSlotPanel.SetActive(true);
        RefreshSlotLabels();
    }

    public void CloseLoadSlots()
    {
        loadSlotPanel.SetActive(false);       
    }

    public void LoadFromSlot(int slot)
    {
        if (!SaveSystem.HasSave(slot))
        {
            Debug.LogWarning($"슬롯 {slot}에 저장된 데이터가 없습니다.");
            return;
        }

        Time.timeScale = 1f; // 일시정지 상태 해제
        Managers.GameSave.LoadGame(slot);
        Debug.Log($"슬롯 {slot}에서 게임 로드");
    }

    private void RefreshSlotLabels()
    {
        for (int i = 0; i < slotButtons.Length; i++)
        {
            var text = slotButtons[i].GetComponentInChildren<TMP_Text>();
            if (SaveSystem.HasSave(i))
                text.text = $"슬롯 {i + 1} (저장됨)";
            else
                text.text = $"슬롯 {i + 1} (빈칸)";
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
