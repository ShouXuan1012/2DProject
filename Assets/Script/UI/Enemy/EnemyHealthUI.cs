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

    public void SetFillDirection(bool isInverted)
    {
        if (healthSlider != null)
        {
            healthSlider.direction = isInverted
                ? Slider.Direction.RightToLeft
                : Slider.Direction.LeftToRight;
        }
    }
}
