using UnityEngine;

public class MainCharacterController : BaseCharacterController
{       
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
        if (isGrounded && Managers.Input.SkillPressed)
        {
            bool nextState = !Managers.Gravity.GetGravityState();
            Managers.Gravity.FlipGravity(nextState);
        }
    }
}
