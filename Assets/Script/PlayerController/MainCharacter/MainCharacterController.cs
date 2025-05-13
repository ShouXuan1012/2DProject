using UnityEngine;

public class MainCharacterController : BaseCharacterController
{       
    private MainCharacterAnimator animator;

    protected override void Awake()
    {
        base.Awake(); // BaseCharacterController의 Awake() 호출
        animator = GetComponent<MainCharacterAnimator>();      
    }
   
    protected override void Update()
    {
        base.Update(); // BaseCharacterController의 Update() 호출
        animator.SetAnimation(rb.velocity.x, rb.velocity.y, isGrounded, isGravityInverted);
    }

    protected override void UseSkill()
    {
        if (isGrounded && Managers.Input.GravityFlipPressed)
        {
            isGravityInverted = !isGravityInverted;

            // 중력 반전 적용 (전역 물리 엔진)
            Physics2D.gravity = isGravityInverted
                ? new Vector2(0, 9.81f)
                : new Vector2(0, -9.81f);

            // 캐릭터 스프라이트도 반전
            Vector3 scale = transform.localScale;
            scale.y *= -1;
            transform.localScale = scale;
        }
    }
}
