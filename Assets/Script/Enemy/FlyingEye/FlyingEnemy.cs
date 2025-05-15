using UnityEngine;

public class FlyingEnemy : Enemy
{
    [Header("FlingEnemy Stats")]
    [SerializeField] private EnemyBullet bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float floatAmplitude = 0.5f;   // 부유 높이 (진폭)
    [SerializeField] private float floatFrequency = 2f;     // 부유 속도 (주기)
    [SerializeField] private float attackCooldown = 2f;     //공격 속도

    private float lastAttackTime;
    private Vector3 startPos;
    

    protected override void Start()
    {   
        base.Start();        
        startPos = transform.position;
        Managers.Pool.CreatePool(bulletPrefab, 20, 40);
    }

    protected override void Update()
    {
        base.Update();
        FacePlayerIfInRange(attackRange); //  이 한 줄로 방향 반전
        if (Time.time - lastAttackTime > attackCooldown)
        {
            if (IsPlayerInRange()) // 범위 내에 있을 때만 공격
            {
                anim.SetTrigger("Attack");
                lastAttackTime = Time.time;
            }
        }
    }

    protected override void Move()
    {
        float floatY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = startPos + new Vector3(0, floatY, 0);
    }

    protected override void Attack()
    {
        EnemyBullet bulletObj = Managers.Pool.GetFromPool(bulletPrefab);
        bulletObj.transform.position = firePoint.position;

        EnemyBullet bullet = bulletObj.GetComponent<EnemyBullet>();
        bullet.SetDirectionToPlayer();
    }
    private bool IsPlayerInRange()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return false;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        return distance <= attackRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
