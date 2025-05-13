using UnityEngine;
using UnityEngine.Video;

public abstract class BaseCharacterController : MonoBehaviour
{
    [Header("Common Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public Transform groundCheckPoint;
    public Vector2 groundCheckSize;
    public LayerMask groundLayer;

    protected Rigidbody2D rb;
    protected bool isGrounded;
    protected bool isGravityInverted;
   

    protected virtual void Awake()
    {
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
        
    protected virtual void UseSkill() { } // 캐릭터마다 다르니까 추상으로
}
