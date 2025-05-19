using UnityEngine;

public class TriggerImageToggle : MonoBehaviour
{
    [SerializeField] private GameObject imageA; // 비활성화할 이미지
    [SerializeField] private GameObject imageB; // 활성화할 이미지

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (imageA != null) imageA.SetActive(false);
            if (imageB != null) imageB.SetActive(true);

            Debug.Log("[TriggerImageToggle] 이미지 전환 완료");
        }
    }
}
