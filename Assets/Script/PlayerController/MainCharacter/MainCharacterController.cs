using UnityEngine;

public class MainCharacterController : BaseCharacterController
{    
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

        if (isGrounded && Managers.Input.SkillPressed)
        {
            lastSkillTime = Time.time;
            bool nextState = !Managers.Gravity.GetGravityState();
            Managers.Gravity.FlipGravity(nextState);
            // 스킬 발동
            Managers.Time.StartCooldown(gameObject, skillCooldownTime);
            Managers.UI.StartCooldown(skillCooldownTime);
        }
    }
    
}
