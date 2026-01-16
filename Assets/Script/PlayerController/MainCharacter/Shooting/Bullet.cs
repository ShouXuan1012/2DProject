using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage => damage;
    float speed = 10f;

    private bool _returned = false;
    private int damage = 1;
    private float direction;

    public Effect hitEffectPrefab;
    
    private void Start()
    {
        Managers.Pool.CreatePool(hitEffectPrefab, 20, 30, TransformUtil.GetOrCreateTransform("EffectObjects"));
    }

    public void SetDirection(float dir)
    {
        direction = Mathf.Sign(dir); // -1 또는 1만 허용, 이동 방향 반영                                     
        transform.localScale = new Vector2(direction * Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }
    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }
    private void OnEnable()
    {
        _returned = false;
    }    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_returned) return;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Effect effect = Managers.Pool.GetFromPool(hitEffectPrefab);
            effect.transform.position = transform.position;
            effect.PlayEffect();

            Enemy enemy = collision.GetComponent<Enemy>(); // 맞은 적에서 컴포넌트 가져옴
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            if (gameObject.activeInHierarchy)
                StartCoroutine(DelayedReturn());
        }
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("BreakableWall"))
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
}
