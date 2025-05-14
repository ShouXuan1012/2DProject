using System.Collections;
using UnityEngine;

public class ThirdCharacterController : BaseCharacterController
{

    [Header("Basic Attack")]
    [SerializeField] private Collider2D BaseAttackCollider;
    [SerializeField] private float BasicAttackCooldown = 3f;
    [SerializeField] private float BasicAttackDuration = 0.5f;
    private float lastBasicAttackTime = -999f;

    [Header("Critical Skill Attack")]
    [SerializeField] private Collider2D CriticalAttackCollider;    
    [SerializeField] private float CriticalAttackCooldown = 5f;
    [SerializeField] private float CriticalAttackDuration = 0.5f;
    private float lastCriticalAttackTime = -999f;

    private Coroutine baseAttackRoutine;
    private Coroutine skillRoutine;
    private CharacterAnimator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<CharacterAnimator>();
    }

    protected override void Update()
    {
        base.Update();
        animator.SetAnimation(rb.velocity.x, rb.velocity.y, isGrounded, Managers.Gravity.GetGravityState());
        Attack(); // 기본 공격
          
    }

    private void Attack()
    {
        if (Time.time - lastBasicAttackTime < BasicAttackCooldown)
            return;

        if (Managers.Input.AttackPressed)
        {
            animator.SetTrigger("BasicAttack");
            lastBasicAttackTime = Time.time;
            if (baseAttackRoutine == null)
                baseAttackRoutine = StartCoroutine(BasicAttackRoutine(BasicAttackDuration));
        }
    }   

    protected override void UseSkill()
    {
        if (Time.time - lastCriticalAttackTime < CriticalAttackCooldown)
            return;

        if (Managers.Input.SkillPressed)
        {
            animator.SetTrigger("CriticalAttack");
            lastCriticalAttackTime = Time.time;
            if (skillRoutine == null)
                skillRoutine = StartCoroutine(CriticalAttackRoutine(CriticalAttackDuration));
        }
    }

    private IEnumerator BasicAttackRoutine(float duration)
    {
        BaseAttackCollider.enabled = true;
        yield return new WaitForSeconds(duration);
        BaseAttackCollider.enabled = false;
        baseAttackRoutine = null;
    }

    private IEnumerator CriticalAttackRoutine(float duration)
    {
        CriticalAttackCollider.enabled = true;
        yield return new WaitForSeconds(duration);
        CriticalAttackCollider.enabled = false;
        skillRoutine = null;
    }
}
