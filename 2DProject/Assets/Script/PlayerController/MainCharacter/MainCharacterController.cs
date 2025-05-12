using System.Collections;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public Transform groundCheckPoint;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    public LayerMask groundLayer;

    private Rigidbody2D rb;       
    private MainCharacterAnimator animator;
    private bool isGrounded;
        
    private bool isGravityInverted = false;
   

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();        
        animator = GetComponent<MainCharacterAnimator>();
    }

    void Update()
    {
        CheckGround();
        HandleMovement();
        animator.SetAnimation(rb.velocity.x, rb.velocity.y, isGrounded,isGravityInverted);        
        Managers.Input.ClearInputs();        
    }


    void HandleMovement()
    {
        // 이동
        rb.velocity = new Vector2(Managers.Input.MoveInput.x * moveSpeed, rb.velocity.y);
        float moveX = rb.velocity.x;
        if (Mathf.Abs(moveX) > 0.01f)
        {
            Vector3 scale = transform.localScale;
            scale.x = moveX < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            transform.localScale = scale;
        }      

        // 점프
        if (Managers.Input.JumpPressed && isGrounded)
        {
            float actualJumpForce = isGravityInverted ? -jumpForce : jumpForce;
            rb.velocity = new Vector2(rb.velocity.x, actualJumpForce);
        }

        // 중력 반전
        if (Managers.Input.GravityFlipPressed && isGrounded)
        {
            FlipGravity();
           
        }
    }    

    void FlipGravity()
    {       
        isGravityInverted = !isGravityInverted;
        Physics2D.gravity = isGravityInverted ? new Vector2(0, 9.81f) : new Vector2(0, -9.81f);

        Vector3 scale = transform.localScale;
        scale.y *= -1;
        transform.localScale = scale;
    }

    void CheckGround()
    {
        Vector2 checkPos = groundCheckPoint.position;
        Vector2 checkDir = isGravityInverted ? Vector2.up : Vector2.down;

        RaycastHit2D hit = Physics2D.BoxCast(
            checkPos,
            groundCheckSize,
            0f,
            checkDir,
            0.1f,
            groundLayer
        );

        isGrounded = hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
    }
}
