using UnityEngine;

public class CharacterChoiceUI : MonoBehaviour
{
    [SerializeField] private GameObject[] choices; // Choice1,2,3
    private int currentIndex = 0;

    public void Show(int startIndex)
    {
        foreach (var choice in choices)
            choice.SetActive(true);

        currentIndex = Mathf.Clamp(startIndex, 0, choices.Length - 1);
        UpdateIndicator();
    }

    public void Hide()
    {
        foreach (var choice in choices)
            choice.SetActive(false);
    }

    public void Move(int direction)
    {
        currentIndex = (currentIndex + direction + choices.Length) % choices.Length;
        UpdateIndicator();
    }

    public int GetSelectedIndex() => currentIndex;

    private void UpdateIndicator()
    {
        for (int i = 0; i < choices.Length; i++)
            choices[i].SetActive(i == currentIndex);
    }
    public void PauseTime()
    {
        float slowScale = 0.2f;
        Time.timeScale = slowScale;
        Time.fixedDeltaTime = 0.02f * slowScale;
    }

    public void ResumeTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
