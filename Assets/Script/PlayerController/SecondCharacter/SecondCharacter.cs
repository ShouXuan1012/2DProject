using System.Collections;
using UnityEngine;

public class SecondCharacterController : BaseCharacterController
{
    [Header("Skill")]    
    [SerializeField] private float invincibleDuration = 5f;

    private float lastSkillTime = -999f;
    private CharacterAnimator animator;
   
    protected override void Awake()
    {
        base.Awake(); // BaseCharacterController의 Awake() 호출
        animator = GetComponent<CharacterAnimator>();
    }

    protected override void Update()
    {
        base.Update(); // BaseCharacterController의 Update() 호출
        animator.SetAnimation(rb.velocity.x, rb.velocity.y, isGrounded, Managers.Gravity.GetGravityState());
    }
    

    protected override void UseSkill()
    {
        if (Managers.Time.IsCooldown(gameObject)) return;

        if (Managers.Input.SkillPressed)
        {
            lastSkillTime = Time.time;

            invincibleEndTime = Time.time + invincibleDuration; // 무적 시간 설정

            if (invincibilityCoroutine != null)
                StopCoroutine(invincibilityCoroutine); // 기존 무적 코루틴 중지

            invincibilityCoroutine = StartCoroutine(InvincibilityFlash(invincibleDuration));

            Debug.Log("고양이 스킬 발동! 5초간 무적");
            Managers.Time.StartCooldown(gameObject, skillCooldownTime);
            Managers.UI.StartCooldown(skillCooldownTime);
        }
    }
}