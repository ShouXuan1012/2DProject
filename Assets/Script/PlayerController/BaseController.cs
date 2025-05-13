using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public abstract class BaseCharacterController : MonoBehaviour
{
    [Header("Common Movement Settings")]
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float jumpForce = 7f;
    [SerializeField] protected int maxHp;
    protected int currentHp;
    protected bool isInvincible = false;

    public Transform groundCheckPoint;
    public Vector2 groundCheckSize;
    public LayerMask groundLayer;

    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;
    protected bool isGrounded;
    protected bool isGravityInverted;

    public bool isDead { get; protected set; } = false;

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
            float actualJumpForce = isGravityInverted ? -jumpForce : jumpForce;
            rb.velocity = new Vector2(rb.velocity.x, actualJumpForce);
        }
    }

    protected virtual void CheckGround()
    {
        Vector2 checkPos = groundCheckPoint.position;
        Vector2 checkDir = isGravityInverted ? Vector2.up : Vector2.down;

        RaycastHit2D hit = Physics2D.BoxCast(
            checkPos, groundCheckSize, 0f, checkDir, 0.1f, groundLayer
        );

        isGrounded = hit.collider != null;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"[Trigger] {gameObject.name} → {collision.gameObject.name}, Tag: {collision.tag}");
        if (isInvincible) return;

        if (collision.CompareTag("EnemyAttack"))
        {
            TakeDamage(1);
        }
    }

    protected virtual void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"[Damage] {gameObject.name} HP: {currentHp}");

        if(currentHp == 0)
        {
            Die();
            return;
        }
        StartCoroutine(InvincibilityFlash());
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        isDead = true;
        gameObject.SetActive(false);
    }

    protected IEnumerator InvincibilityFlash()
    {
        isInvincible = true;


        Color originalColor = spriteRenderer.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);

        float flashTime = 2.0f; // 무적 지속 시간
        spriteRenderer.color = transparentColor;

        yield return new WaitForSeconds(flashTime);

        spriteRenderer.color = originalColor;
        isInvincible = false;
    }

    protected virtual void UseSkill() { } // 캐릭터마다 다르니까 추상으로
}
