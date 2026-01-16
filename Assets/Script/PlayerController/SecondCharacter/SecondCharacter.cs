using System.Collections;
using UnityEngine;

public class SecondCharacterController : BaseCharacterController
{
    [Header("Skill")]    
    [SerializeField] private float invincibleDuration = 5f;

    [Header("For Checking Narrow Spaces")]
    [SerializeField] private Transform boxCastOrigin; // 빈 오브젝트 (위치 기준)
    [SerializeField] private Vector2 boxSize = new Vector2(0.4f, 0.2f); // 검사할 상자의 크기
    [SerializeField] private float castDistance = 0.1f; // 위로 얼마만큼 쏠 건지
    [SerializeField] private LayerMask ceilingMask; // 예: "Ground"
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

    public override bool CanExitNarrowSpace()
    {
        if (boxCastOrigin == null)
        {
            Debug.LogWarning("BoxCast Origin이 설정되지 않았습니다.");
            return true;
        }

        Vector2 origin = boxCastOrigin.position;
        Vector2 direction = Vector2.up;

        RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, 0f, direction, castDistance, ceilingMask);

        return hit.collider == null;
    }

    private void OnDrawGizmosSelected()
    {
        if (boxCastOrigin == null) return;

        Gizmos.color = Color.cyan;
        Vector3 castOrigin = boxCastOrigin.position;
        Vector3 castEnd = castOrigin + Vector3.up * castDistance;

        // BoxCast 시 검사되는 영역 시각화
        Gizmos.matrix = Matrix4x4.TRS(castEnd, Quaternion.identity, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
