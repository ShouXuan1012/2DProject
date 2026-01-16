using UnityEngine;

public class CriticalAttack : MonoBehaviour
{
    [SerializeField] private int skillDamage = 5;
    [SerializeField] private Effect deathEffectPrefab; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(skillDamage);
        }
        else if (collision.CompareTag("BreakableWall"))
        {
            // 이펙트 생성
            Effect effect = Managers.Pool.GetFromPool(deathEffectPrefab);
            effect.transform.position = collision.transform.position;
            effect.PlayEffect();

            Destroy(collision.gameObject); // 벽 제거
        }
    }
}
