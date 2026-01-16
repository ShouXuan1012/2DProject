using UnityEngine;

public class ShowTutorialText : MonoBehaviour
{
    [SerializeField] private GameObject targetTextObject; // 보여줄 텍스트 오브젝트
    [SerializeField] private float time;

    private bool isShown = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isShown && other.CompareTag("Player"))
        {
            ShowOnlyThisText();
            isShown = true;
        }
    }

    private void ShowOnlyThisText()
    {
        // 1. 모든 텍스트 비활성화
        foreach (Transform child in targetTextObject.transform.parent)
        {
            child.gameObject.SetActive(false);
        }

        // 2. 내가 보여줄 텍스트만 활성화
        targetTextObject.SetActive(true);

        // 3. 시간 지나면 다시 숨기기
        CancelInvoke(nameof(Hide));
        Invoke(nameof(Hide), time);
    }

    private void Hide()
    {
        targetTextObject.SetActive(false);
    }
}
