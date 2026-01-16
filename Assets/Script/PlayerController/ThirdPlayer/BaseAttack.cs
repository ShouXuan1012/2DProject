using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    [SerializeField] private int skillDamage = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"[충돌 감지] {gameObject.name} → {collision.name}");

        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(skillDamage);
        }        
    }
}
