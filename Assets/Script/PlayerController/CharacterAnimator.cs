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
    public void SetAnimation(float speedX, float speedY, bool isground, bool isInverted)
    {
        animator.SetFloat("IsRun", Mathf.Abs(speedX));
        animator.SetFloat("IsJump",speedY);
        animator.SetBool("IsGrounded", isground);
        animator.SetBool("IsInverted", isInverted);
    }    
}
