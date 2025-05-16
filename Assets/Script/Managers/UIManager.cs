using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //체력 UI
    private List<GameObject> hearts = new();
    private Transform heartContainer;
    private GameObject heartPrefab;

    //스킬 쿨타임 UI
    private Transform skillUIParent;
    private GameObject skillIconPrefab;
    private GameObject currentSkillIcon; // 하나만 관리
    private SkillCooldownUI cooldownUI;

    private void Awake()
    {
        // 1. 하트 아이콘 프리팹 로드
        heartPrefab = Resources.Load<GameObject>("Prefabs/UI/Player/PlayerHp");

        if (heartPrefab == null)
        {
            Debug.LogError("heartPrefab 못 찾음");
            return;
        }

        // 2. PlayerHp 트랜스폼 찾기
        GameObject container = GameObject.Find("PlayerHp");
        if (container == null)
        {
            Debug.LogError("PlayerHp 오브젝트 못 찾음 (Canvas 안에 있어야 함)");
            return;
        }

        heartContainer = container.transform;

        //3. 스킬 이미지 찾기
        skillIconPrefab = Resources.Load<GameObject>("Prefabs/UI/Player/SkillIcon");
        if (skillIconPrefab == null)
            Debug.LogError("SkillIcon 프리팹을 못 찾음");

        GameObject skillContainer = GameObject.Find("Skill");
        if (skillContainer != null)
            skillUIParent = skillContainer.transform;
        else
            Debug.LogError("Skill 오브젝트를 못 찾음");
    }

    private void Start()
    {
        int maxHp = Managers.Character.CurrentCharacter
            .GetComponent<BaseCharacterController>().MaxHp;

        for (int i = 0; i < maxHp; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            hearts.Add(heart);
        }
    }

    public void InitHearts(int maxHp)
    {
        foreach (Transform child in heartContainer)
            Destroy(child.gameObject);
        hearts.Clear();

        for (int i = 0; i < maxHp; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer); // 이건 개별 하트 프리팹
            hearts.Add(heart);
        }
    }

    public void UpdateHealth(int currentHp)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            var icon = hearts[i].GetComponent<HpIcon>();
            if (icon != null)
                icon.SetFull(i < currentHp); // 현재 체력 이하까지만 Full
        }
    }

    public void InitSkillIcon(Sprite skillSprite, float remainingCooldown = 0f, float totalCooldown = 0f)
    {
        if (currentSkillIcon != null)
            Destroy(currentSkillIcon);

        currentSkillIcon = Instantiate(skillIconPrefab, skillUIParent);
        cooldownUI = currentSkillIcon.GetComponent<SkillCooldownUI>();

        // 아이콘 이미지 변경
        var img = currentSkillIcon.transform.Find("SkillIcon")?.GetComponent<Image>();
        if (img != null)
            img.sprite = skillSprite;

        // 캐릭터가 쿨타임 중이면 UI에도 즉시 반영
        if (remainingCooldown > 0f && totalCooldown > 0f)
        {
            ForceCooldownUI(remainingCooldown, totalCooldown);
        }
    }

    public void StartCooldown(float duration)
    {
        if (cooldownUI != null)
            cooldownUI.StartCooldown(duration);
    }

    public void ForceCooldownUI(float remaining, float total)
    {
        if (cooldownUI != null)
            cooldownUI.ForceSetCooldown(remaining, total);
    }
}
