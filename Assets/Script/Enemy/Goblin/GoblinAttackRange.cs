using UnityEngine;

public class GoblinAttackRange : MonoBehaviour
{
    private GoblinEnemy goblin;

    private void Awake()
    {
        goblin = GetComponentInParent<GoblinEnemy>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time - goblin.LastAttackTime >= goblin.AttackCooldown)
        {
            BaseCharacterController player = other.GetComponent<BaseCharacterController>();
            if (player != null)
            {
                player.TakeDamage(goblin.AttackDamage);
                goblin.LastAttackTime = Time.time;
            }
        }
    }
}
