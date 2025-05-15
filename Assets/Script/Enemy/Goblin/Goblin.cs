using System.Collections;
using UnityEngine;

public class GoblinEnemy : Enemy
{
    private enum GoblinState { Patrol, Chase, Attack }
    private GoblinState currentState;

    [Header("Movement")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 4f;    

    [Header("Detection")]
    [SerializeField] private Vector2 playerCheckBoxSize = new Vector2(6f, 1f);
    [SerializeField] private Vector2 attackCheckBoxSize = new Vector2(1.5f, 1f);
    [SerializeField] private Transform detectionOrigin;

    [Header("Attack")]
    [SerializeField] protected Collider2D attackRangeCollider;
    [SerializeField] protected int attackDamage = 1;
    [SerializeField] protected float attackCooldown = 2f;
    protected float lastAttackTime = -999f;

    [Header("HitEffect")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashDuration = 0.1f;
    private Coroutine flashRoutine;

    public int AttackDamage => attackDamage;
    public float AttackCooldown => attackCooldown;
    public float LastAttackTime { get => lastAttackTime; set => lastAttackTime = value; }


    private Animator animator;   
    private bool isMovingRight = true;
    private Coroutine patrolRoutine;
    private bool isStanding = false;
    private bool isAttacking = false;
    private GoblinState previousState = GoblinState.Patrol;
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }
    protected override void Start()
    {
        base.Start();     
        attackRangeCollider.enabled = false;
    }

    protected override void Update()
    {
        base.Update();

        GoblinState nextState;

        if (CheckBoxForPlayer(attackCheckBoxSize))
            nextState = GoblinState.Attack;
        else if (CheckBoxForPlayer(playerCheckBoxSize))
            nextState = GoblinState.Chase;
        else
            nextState = GoblinState.Patrol;

        if (nextState != previousState)
        {
            SetState(nextState);
            previousState = nextState;
        }
    }

    protected override void Move()
    {
        if (isDead) return;

        switch (currentState)
        {
            case GoblinState.Patrol:
                if (!isStanding) Patrol();
                else rb.velocity = new Vector2(0, rb.velocity.y);
                break;

            case GoblinState.Chase:
                if (!isAttacking) // ← 공격 중이 아닐 때만 추격
                    ChasePlayer();
                else
                    rb.velocity = new Vector2(0, rb.velocity.y); // 추격 정지
                break;

            case GoblinState.Attack:
                rb.velocity = new Vector2(0, rb.velocity.y);
                FacePlayerIfInRange(attackCheckBoxSize.x);                
                Attack();                
                break;
        }

        UpdateAnimationState();
    }

    protected override void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            animator.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }
    }

    //애니메이션 이벤트용
    public void BeginAttack()
    {
        isAttacking = true;
        rb.velocity = new Vector2(0, rb.velocity.y); // 공격 중 이동 정지
    }

    public void EnableAttackCollider()
    {
        attackRangeCollider.enabled = true;
    }

    public void DisableAttackCollider()
    {
        attackRangeCollider.enabled = false;
    }

    public void EndAttack()
    {
        isAttacking = false;
        animator.ResetTrigger("Hit");
    }


    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (isAttacking)
        {
            // 피격 애니메이션은 막고, 스프라이트만 깜빡이기
            if (flashRoutine != null)
                StopCoroutine(flashRoutine);

            flashRoutine = StartCoroutine(FlashWhite());
            return;
        }

        animator.SetTrigger("Hit");
    }

    private void SetState(GoblinState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        if (currentState == GoblinState.Patrol)
        {
            if (patrolRoutine == null)
                patrolRoutine = StartCoroutine(PatrolBehaviorLoop());
        }
        else
        {
            if (patrolRoutine != null)
            {
                StopCoroutine(patrolRoutine);
                patrolRoutine = null;
                isStanding = false; // 이동 상태로 초기화
            }
        }
    }

    private void Patrol()
    {
        float dir = isMovingRight ? 1f : -1f;
        rb.velocity = new Vector2(dir * patrolSpeed, rb.velocity.y);

        // 벽 감지
        if (IsWallInFront())
            isMovingRight = !isMovingRight;

        // 방향 반영
        Vector3 scale = transform.localScale;
        scale.x = isMovingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private bool IsWallInFront()
    {
        Vector2 origin = transform.position;
        Vector2 direction = isMovingRight ? Vector2.right : Vector2.left;
        float distance = 0.5f; // 벽 감지 거리
        int wallLayer = LayerMask.GetMask("Ground"); // 벽 레이어 설정

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, wallLayer);
        Debug.DrawRay(origin, direction * distance, Color.green); // 디버그용

        return hit.collider != null;
    }

    private void ChasePlayer()
    {
        if (playerTransform == null) return;

        float dir = Mathf.Sign(playerTransform.position.x - transform.position.x);
        rb.velocity = new Vector2(dir * chaseSpeed, rb.velocity.y);

        Vector3 scale = transform.localScale;
        scale.x = dir < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private bool CheckBoxForPlayer(Vector2 size)
    {
        Collider2D hit = Physics2D.OverlapBox(detectionOrigin.position, size, 0f, LayerMask.GetMask("Player"));

        if (hit != null && hit.gameObject.activeInHierarchy)
        {
            BaseCharacterController player = hit.GetComponent<BaseCharacterController>();
            if (player != null && !player.isDead)
            {
                playerTransform = player.transform;
                return true;
            }
        }

        return false;
    }



    private IEnumerator PatrolBehaviorLoop()
    {
        while (currentState == GoblinState.Patrol)
        {
            isStanding = Random.value < 0.5f; // 50% 확률로 서기 or 걷기

            float waitTime = Random.Range(1f, 2.5f); // 행동 유지 시간
            yield return new WaitForSeconds(waitTime);
        }
    }

    //공격중 피격시 효과
    private IEnumerator FlashWhite()
    {
        spriteRenderer.color = Color.red; // 원래 색으로 (없다면 Color.white로도 OK)
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = Color.white;
        flashRoutine = null;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(detectionOrigin.position, playerCheckBoxSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(detectionOrigin.position, attackCheckBoxSize);
    }

    private void UpdateAnimationState()
    {
        if (animator == null) return;

        // 수평 속도 기준으로 걷는지 판단 (작은 값은 정지 간주)
        bool isWalking = Mathf.Abs(rb.velocity.x) > 0.01f;
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isAttacking", isAttacking);
    }
}
