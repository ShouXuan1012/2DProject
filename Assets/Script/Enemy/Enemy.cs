using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected int maxHp;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected Effect deathEffectPrefab;
    

    protected int currentHp;
    protected bool isDead = false;

    protected Animator anim;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        currentHp = maxHp;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();        
    }
    protected virtual void Start()
    {
        Managers.Pool.CreatePool(deathEffectPrefab, 20, 40, TransformUtil.GetOrCreateTransform("EffectObjects"));
    }
    protected virtual void Update()
    {
        if (!isDead)
        {
            Move();
        }
    }

    public virtual void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0) Die();
        anim.SetTrigger("Hit");
    }

    protected virtual void Die()
    {
        isDead = true;

        Effect effect = Managers.Pool.GetFromPool(deathEffectPrefab);
        effect.transform.position = transform.position;
        effect.PlayEffect(); // 꼭 복사본에 실행!

        Destroy(gameObject); // 죽은 적 제거
    }


    protected abstract void Move(); // 걷는 적, 나는 적이 각자 구현
    protected abstract void Attack(); // 공격하는 적만 구현
}
