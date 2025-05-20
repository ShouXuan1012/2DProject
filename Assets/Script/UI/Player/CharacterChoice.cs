using UnityEngine;

public class CharacterChoiceUI : MonoBehaviour
{
    [SerializeField] private GameObject[] choices; // Choice1,2,3
    [SerializeField] private GameObject[] portraits;// FirstCharacter, SecondCharacter, ThirdCharacter
    private int currentIndex = 0;

    public void Show(int startIndex)
    {
        currentIndex = Mathf.Clamp(startIndex, 0, choices.Length - 1);

        var charManager = Managers.Character;

        for (int i = 0; i < choices.Length; i++)
        {
            bool isDead = charManager.CurrentCharacterList[i]
                .GetComponent<BaseCharacterController>().isDead;

            bool alive = !isDead;

            // 선택창 & 초상화 모두 비활성화
            choices[i].SetActive(alive);
            portraits[i].SetActive(alive);

            if (i == currentIndex && isDead)
                currentIndex = GetNextAliveIndex(i, charManager);
        }

        UpdateIndicator();
    }

    public void Hide()
    {
        foreach (var choice in choices)
            choice.SetActive(false);
    }

    public void Move(int direction)
    {
        var charManager = FindObjectOfType<CharacterManager>();
        int attempts = 0;
        int nextIndex = currentIndex;

        // 죽은 캐릭터는 건너뜀
        do
        {
            nextIndex = (nextIndex + direction + choices.Length) % choices.Length;
            attempts++;
        }
        while (charManager.CurrentCharacterList[nextIndex]
                  .GetComponent<BaseCharacterController>().isDead
               && attempts < choices.Length);

        currentIndex = nextIndex;
        UpdateIndicator();
    }

    public int GetSelectedIndex() => currentIndex;

    private void UpdateIndicator()
    {
        for (int i = 0; i < choices.Length; i++)
            choices[i].SetActive(i == currentIndex);
    }

    private int GetNextAliveIndex(int fromIndex, CharacterManager charManager)
    {
        for (int i = 1; i < choices.Length; i++)
        {
            int next = (fromIndex + i) % choices.Length;
            if (!charManager.CurrentCharacterList[next]
                    .GetComponent<BaseCharacterController>().isDead)
                return next;
        }
        return fromIndex; // 모두 죽었으면 그대로
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
