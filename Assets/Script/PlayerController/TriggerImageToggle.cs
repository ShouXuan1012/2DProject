using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerImageToggle : MonoBehaviour
{
    [SerializeField] private GameObject imageA; // 진입 시 숨길 이미지
    [SerializeField] private GameObject imageB; // 진입 시 보여줄 이미지
    [SerializeField] private string nextSceneName; // 이동할 씬 이름
    private bool _playerInTrigger = false;

    private void Update()
    {
        if (_playerInTrigger && Input.GetKeyDown(KeyCode.DownArrow))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (imageA != null) imageA.SetActive(false);
            if (imageB != null) imageB.SetActive(true);
            _playerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (imageA != null) imageA.SetActive(true);
            if (imageB != null) imageB.SetActive(false);
            _playerInTrigger = false;
        }
    }

}
