using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotUIManager : MonoBehaviour
{
    [Header("Panels")]    
    public GameObject saveSlotPanel;        // ½½·Ô 3°³ ÆÐ³Î
    public GameObject overwritePopup;       // µ¤¾î¾²±â ÆË¾÷

    [Header("Overwrite UI")]
    public TextMeshProUGUI overwriteText;
    private int selectedSlot = -1;

    [Header("½½·Ô ¹öÆ°µé")]
    public Button[] slotButtons;            // ½½·Ô 0, 1, 2¿¡ ¿¬°áµÈ ¹öÆ°µé

    private void Start()
    {
        saveSlotPanel.SetActive(false);
        overwritePopup.SetActive(false);        
    }

    public void OpenSaveSlots()
    {       
        saveSlotPanel.SetActive(true);
        RefreshSlotLabels();
    }

    public void CloseSaveSlots()
    {
        saveSlotPanel.SetActive(false);       
    }

    public void SaveToSlot(int slot)
    {
        if (SaveSystem.HasSave(slot))
        {
            selectedSlot = slot;
            overwriteText.text = $"½½·Ô {slot + 1}¿¡ ÀÌ¹Ì ÀúÀåÀÌ ÀÖ½À´Ï´Ù.\nµ¤¾î¾µ±î¿ä?";
            overwritePopup.SetActive(true);
        }
        else
        {
            ConfirmSave(slot);
        }
    }

    public void ConfirmOverwrite()
    {
        ConfirmSave(selectedSlot);
        overwritePopup.SetActive(false);
        CloseSaveSlots();
    }

    public void CancelOverwrite()
    {
        overwritePopup.SetActive(false);
        selectedSlot = -1;
    }

    private void ConfirmSave(int slot)
    {
        Managers.GameSave.SaveGame(slot);
        Debug.Log($"½½·Ô {slot}¿¡ ÀúÀå ¿Ï·á");
    }

    private void RefreshSlotLabels()
    {
        for (int i = 0; i < slotButtons.Length; i++)
        {
            var text = slotButtons[i].GetComponentInChildren<TMP_Text>();

            if (SaveSystem.HasSave(i))
                text.text = $"½½·Ô {i + 1} (ÀúÀåµÊ)";
            else
                text.text = $"½½·Ô {i + 1} (ºóÄ­)";
        }
    }
}
