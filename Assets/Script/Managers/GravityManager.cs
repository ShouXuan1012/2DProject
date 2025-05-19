using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public string[] gravityAffectedTags = { "Player", "Enemy" };
    private CameraManager cameraManager;
    private bool isGravityInverted = false;

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
    }
    public void FlipGravity(bool inverted)
    {        
        isGravityInverted = inverted;

        Physics2D.gravity = inverted ? new Vector2(0, 9.81f) : new Vector2(0, -9.81f);

        foreach (string tag in gravityAffectedTags)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in objects)
            {
                FlipVisual(obj.transform, inverted);
            }
        }
       cameraManager.FlipCameraRotation(inverted);
    }

    public bool GetGravityState() => isGravityInverted;

    public void ApplyGravityVisual(Transform t)
    {
        FlipVisual(t, isGravityInverted);
    }

    private void FlipVisual(Transform t, bool isInverted)
    {
        Vector3 scale = t.localScale;
        scale.y = Mathf.Abs(scale.y) * (isInverted ? -1 : 1);
        t.localScale = scale;
    }
}
