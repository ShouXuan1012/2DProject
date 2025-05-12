using System.Collections;
using UnityEngine;

public class SecondCharacterController : MonoBehaviour
{
    public float moveSpeed = 4.5f;
    public float jumpForce = 6.5f;

    public Transform groundCheckPoint;
    public Vector2 groundCheckSize = new Vector2(0.4f, 0.05f);
    public LayerMask groundLayer;

    private Rigidbody2D rb;   
    private bool isGrounded;

    private bool isGravityInverted = false;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();      
    }

    void Update()
    {
        CheckGround();
        HandleMovement();        
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
