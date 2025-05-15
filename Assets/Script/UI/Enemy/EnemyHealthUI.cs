using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Slider healthSlider;
    public Transform target; // 적의 Transform
    public Vector3 offset = new Vector3(0, 1.5f, 0); // 머리 위 위치

    private Enemy enemy;

    void Start()
    {
        enemy = target.GetComponent<Enemy>();
        healthSlider = GetComponentInChildren<Slider>();
        healthSlider.minValue = 0;
        healthSlider.maxValue = 1;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // 적이 사라졌으면 UI도 제거
            return;
        }
    }

    void LateUpdate()
    {
        if (enemy != null && healthSlider != null)
        {
            float ratio = (float)enemy.CurrentHp / enemy.MaxHp;
            healthSlider.value = ratio;
        }

        // 체력바 위치를 적 머리 위로 고정
        transform.position = target.position + offset;
        transform.forward = Camera.main.transform.forward;
    }
}
