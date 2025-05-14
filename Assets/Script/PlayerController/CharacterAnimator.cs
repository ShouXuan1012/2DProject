using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{    
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void SetAnimation(float speedX, float speedY, bool isGround, bool isInverted)
    {
        animator.SetFloat("IsRun", Mathf.Abs(speedX));
        animator.SetFloat("IsJump",speedY);
        animator.SetBool("IsGrounded", isGround);
        animator.SetBool("IsInverted", isInverted);
    }    

    public void SetTrigger(string name)
    {
        animator.SetTrigger(name);
    }
}
