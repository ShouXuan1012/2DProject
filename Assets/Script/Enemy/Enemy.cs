using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected int maxHp;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected Effect deathEffectPrefab;

    [Header("UI")]
    [SerializeField] protected GameObject hpUIPrefab;
    [SerializeField] protected float UIHeight;
    [SerializeField] protected float UIWidth;
    protected EnemyHealthUI hpUIInstance;

    protected Transform playerTransform;
    protected int currentHp;
    protected bool isDead = false;

    protected Animator anim;
    protected Rigidbody2D rb;

    public int MaxHp => maxHp;
    public int CurrentHp => currentHp;

    protected virtual void Awake()
    {
        currentHp = maxHp;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();        
        if (hpUIPrefab != null)
        {
            GameObject ui = Instantiate(hpUIPrefab);
            ui.transform.SetParent(this.transform, true);
            hpUIInstance = ui.GetComponent<EnemyHealthUI>();            
        }        
    }
    protected virtual void Start()
    {
        Managers.Pool.CreatePool(deathEffectPrefab, 20, 40, TransformUtil.GetOrCreateTransform("EffectObjects"));
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            playerTransform = player.transform;
    }
    protected virtual void Update()
    {
        if (!isDead && hpUIInstance != null)
        {
            float direction = Managers.Gravity.GetGravityState() ? -1f : 1f; // 반전 시 -1
            Vector3 uiPos = transform.position + new Vector3(UIWidth, UIHeight * direction, 0);
            hpUIInstance.Init(uiPos);
            hpUIInstance.SetFillDirection(Managers.Gravity.GetGravityState());
        }
        if (!isDead)
        {
            Move();
        }
    }

    protected void FacePlayerIfInRange(float range)
    {
        if (playerTransform == null) return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if (distance > range) return; // 범위 바깥이면 반전 안 함

        float dir = playerTransform.position.x - transform.position.x;
        if (Mathf.Abs(dir) > 0.1f)
        {
            Vector3 scale = transform.localScale;
            scale.x = dir < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            transform.localScale = scale;
        }      
    }

    public virtual void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (hpUIInstance != null)
            hpUIInstance.UpdateHealth((float)currentHp / maxHp);
        if (currentHp <= 0) Die();
        anim.SetTrigger("Hit");
    }

    protected virtual void Die()
    {        
        isDead = true;

        Effect effect = Managers.Pool.GetFromPool(deathEffectPrefab);        
        effect.transform.position = transform.position;
        effect.PlayEffect(); // 꼭 복사본에 실행!

        if (hpUIInstance != null) Destroy(hpUIInstance.gameObject);
        Destroy(gameObject); // 죽은 적 제거
    }


    protected abstract void Move(); // 걷는 적, 나는 적이 각자 구현
    protected abstract void Attack(); // 공격하는 적만 구현
}
