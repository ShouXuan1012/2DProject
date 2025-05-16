using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillCooldownUI : MonoBehaviour
{
    [SerializeField] private Image cooldownOverlay; // 덮는 검정 이미지

    public void StartCooldown(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(CooldownRoutine(duration));
    }

    private IEnumerator CooldownRoutine(float duration)
    {
        float elapsed = 0f;
        cooldownOverlay.fillAmount = 1f; // 처음엔 꽉 덮인 상태

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float ratio = Mathf.Clamp01(1f - (elapsed / duration));
            cooldownOverlay.fillAmount = ratio;
            yield return null;
        }

        cooldownOverlay.fillAmount = 0f; // 쿨타임 끝나면 사라짐
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
