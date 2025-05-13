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

    public HitEffect hitEffectPrefab;

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

    private void OnBecameInvisible()
    {
        if (_returned) return;

        _returned = true;
        PoolManager.Instance.ReturnPool(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_returned) return;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HitEffect effect = Managers.Pool.GetFromPool(hitEffectPrefab);
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
