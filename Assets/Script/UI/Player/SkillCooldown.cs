using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class SkillCooldownUI : MonoBehaviour
{
    [SerializeField] private Image cooldownOverlay; // 덮는 검정 이미지
    [SerializeField] private TextMeshProUGUI cooldownText;

    public void StartCooldown(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(CooldownRoutine(duration));
    }

    private IEnumerator CooldownRoutine(float duration)
    {
        float remaining = duration;
        cooldownText.gameObject.SetActive(true);

        while (remaining > 0f)
        {
            cooldownOverlay.fillAmount = remaining / duration;
            cooldownText.text = Mathf.CeilToInt(remaining).ToString();
            remaining -= Time.deltaTime;
            yield return null;
        }

        cooldownOverlay.fillAmount = 0f;
        cooldownText.gameObject.SetActive(false);
    }

    public void ForceSetCooldown(float remaining, float total)
    {
        StopAllCoroutines();

        if (cooldownOverlay == null) return;

        float ratio = Mathf.Clamp01(remaining / total);
        cooldownOverlay.fillAmount = ratio;

        // 남은 시간만큼 다시 쿨다운 코루틴 실행
        StartCoroutine(CooldownRoutine(remaining));
    }

}
