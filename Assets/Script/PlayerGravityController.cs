using UnityEngine;

public class PlayerGravityController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("바닥 감지 설정")]
    public Transform groundCheckPoint;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isGravityInverted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Physics2D.gravity = new Vector2(0, -9.81f); // 초기 중력
    }

    void Update()
    {
        HandleInput();
        CheckGround();
    }

    void HandleInput()
    {
        // 좌우 이동
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            float actualJumpForce = isGravityInverted ? -jumpForce : jumpForce;
            rb.velocity = new Vector2(rb.velocity.x, actualJumpForce);
        }

        // 중력 반전
        if (Input.GetKeyDown(KeyCode.G))
        {
            InvertGravity();
        }
    }

    void InvertGravity()
    {
        isGravityInverted = !isGravityInverted;

        Physics2D.gravity = isGravityInverted
            ? new Vector2(0, 9.81f)
            : new Vector2(0, -9.81f);

        // 캐릭터 시각 반전 (y 스케일 반전)
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

    // 시각적으로 바닥 감지 범위를 그려줌
    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null) return;

        Gizmos.color = Color.green;
        Vector3 boxCenter = groundCheckPoint.position;
        Gizmos.DrawWireCube(boxCenter, groundCheckSize);
    }
}
