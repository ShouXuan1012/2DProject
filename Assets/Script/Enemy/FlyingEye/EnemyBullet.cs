using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int Damage => damage;
    public Effect hitEffectPrefab;

    float speed = 5f;

    private bool _returned = false;
    private int damage = 1;
    private float direction;
    
    private Vector2 moveDirection;

    private void Start()
    {
        Managers.Pool.CreatePool(hitEffectPrefab, 20, 30, TransformUtil.GetOrCreateTransform("EffectObjects"));
    }
    void Update()
    {       
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
    private void OnEnable()
    {
        _returned = false;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_returned) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            Effect effect = Managers.Pool.GetFromPool(hitEffectPrefab);
            effect.transform.position = transform.position;
            effect.PlayEffect();

            BaseCharacterController baseCharacter = collision.GetComponent<BaseCharacterController>(); // 맞은 적에서 컴포넌트 가져옴
            if (baseCharacter != null && !baseCharacter.IsInvincible && !baseCharacter.isDead)
            {
                baseCharacter.TakeDamage(damage);
            }

            if (gameObject.activeInHierarchy)
                StartCoroutine(DelayedReturn());
        }
        if(collision.gameObject.CompareTag("Ground"))
        {
            Effect effect = Managers.Pool.GetFromPool(hitEffectPrefab);
            effect.transform.position = transform.position;
            effect.PlayEffect();

            if (gameObject.activeInHierarchy)
                StartCoroutine(DelayedReturn());
        }
    }
    private IEnumerator DelayedReturn()
    {
        yield return null; // 1프레임 기다렸다가 반환 → SetParent 에러 방지

        if (_returned) yield break; // 혹시라도 이미 리턴됐으면 중복 방지

        _returned = true;
        PoolManager.Instance.ReturnPool(this);
    }
    public void SetDirection(float dir)
    {
        direction = Mathf.Sign(dir); // -1 또는 1만 허용, 이동 방향 반영                                     
        transform.localScale = new Vector2(direction * Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }

    public void SetDirectionToPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            moveDirection = (player.transform.position - transform.position).normalized;
            // 총알 회전 방향도 맞추고 싶다면 여기서 LookRotation 처리 가능
        }
        else
        {
            moveDirection = Vector2.left; // fallback
        }
    }
}
