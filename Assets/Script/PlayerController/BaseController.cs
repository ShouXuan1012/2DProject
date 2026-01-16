using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public abstract class BaseCharacterController : MonoBehaviour
{
    [Header("Common Movement Settings")]
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float jumpForce = 7f;
    [SerializeField] protected int maxHp;
    protected int currentHp;   

    public Transform groundCheckPoint;
    public Vector2 groundCheckSize;
    public LayerMask groundLayer;

    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;
    protected bool isGrounded;    

    public bool isDead { get; protected set; } = false;
    public void SetDeadState(bool dead)
    {
        isDead = dead;
        gameObject.SetActive(!dead);
    }

    protected float invincibleEndTime = 0f;

    public bool IsInvincible => Time.time < invincibleEndTime;
    protected Coroutine invincibilityCoroutine;
    public int MaxHp => maxHp;
    public int CurrentHp => currentHp;

    [Header("SkillCooldownTime")]
    [SerializeField] protected float skillCooldownTime;

    protected virtual void Awake()
    {
        currentHp = maxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    protected virtual void Update()
    {       
        CheckGround();
        Move();
        Jump();
        UseSkill();       
    }
    protected virtual void LateUpdate()
    {        
        Managers.Input.ClearInputs();
    }


    protected virtual void Move()
    {
        rb.velocity = new Vector2(Managers.Input.MoveInput.x * moveSpeed, rb.velocity.y);
        if (Mathf.Abs(rb.velocity.x) > 0.01f)
        {
            Vector3 scale = transform.localScale;
            scale.x = rb.velocity.x < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }
    
    protected virtual void Jump()
    {
        if (Managers.Input.JumpPressed && isGrounded)
        {            
            bool inverted = Managers.Gravity.GetGravityState();
            float actualJumpForce = inverted ? -jumpForce : jumpForce;
            rb.velocity = new Vector2(rb.velocity.x, actualJumpForce);
        }
    }

    protected virtual void CheckGround()
    {
        Vector2 checkPos = groundCheckPoint.position;
        Vector2 checkDir = Managers.Gravity.GetGravityState() ? Vector2.up : Vector2.down;

        RaycastHit2D hit = Physics2D.BoxCast(
            checkPos, groundCheckSize, 0f, checkDir, 0.1f, groundLayer
        );

        isGrounded = hit.collider != null;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"[Trigger] {gameObject.name} → {collision.gameObject.name}, Tag: {collision.tag}");
        if (IsInvincible) return;

        if (collision.CompareTag("EnemyAttack"))
        {
            TakeDamage(1);
        }
        else if (collision.CompareTag("Potion"))
        {
            if (currentHp < maxHp)
            {
                currentHp += 1;
                Managers.UI.UpdateHealth(currentHp);
                Debug.Log($"[회복] {gameObject.name} HP: {currentHp}");
                Destroy(collision.gameObject); // 포션 제거
            }

        }
    }

    public virtual void TakeDamage(int damage)
    {
        if (IsInvincible) return; // 무적이면 데미지 무시!

        currentHp -= damage;
        Debug.Log($"[Damage] {gameObject.name} HP: {currentHp}");

        if (currentHp == 0)
        {
            Die();
            return;
        }

        Managers.UI.UpdateHealth(currentHp);

        invincibleEndTime = Time.time + 1.5f; // 무적 시간 연장

        if (invincibilityCoroutine != null)
            StopCoroutine(invincibilityCoroutine); // 중복 방지

        if (!isDead)
            invincibilityCoroutine = StartCoroutine(InvincibilityFlash(1.5f));
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        isDead = true;
        gameObject.SetActive(false);
    }

    protected IEnumerator InvincibilityFlash(float duration)
    {
        Color originalColor = spriteRenderer.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);

        spriteRenderer.color = transparentColor;

        yield return new WaitForSeconds(duration);

        //  무조건 원래 색으로 복원
        spriteRenderer.color = Color.white;

        invincibilityCoroutine = null;
    }

    protected virtual void OnEnable()
    {      

        // 원래 있던 중력, 색상 처리 유지
        Managers.Gravity.ApplyGravityVisual(transform);

        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;
    }
    protected virtual void UseSkill() { } // 캐릭터마다 다르니까 추상으로
        
    public virtual bool CanExitNarrowSpace()
    {
        return true; // 기본은 무조건 허용
    }

}
