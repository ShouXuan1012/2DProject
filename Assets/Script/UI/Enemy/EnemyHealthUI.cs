using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Slider healthSlider;

    public void Init(Vector3 worldPosition)
    {
        transform.position = worldPosition;
        transform.forward = Camera.main.transform.forward; // billboard
    }

    public void UpdateHealth(float ratio)
    {
        healthSlider.value = ratio;
    }
}
