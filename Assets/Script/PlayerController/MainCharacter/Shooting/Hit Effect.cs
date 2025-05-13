using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private string animName;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayEffect()
    {
        gameObject.SetActive(true);
        animator.Play(animName);
        StartCoroutine(DisableAfterAnimation());
    }

    private IEnumerator DisableAfterAnimation()
    {
        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Managers.Pool.ReturnPool(this);
    }
}
