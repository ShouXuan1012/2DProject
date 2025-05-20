using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GravityManager : MonoBehaviour
{
    public string[] gravityAffectedTags = { "Player", "Enemy" };
    private CameraManager cameraManager;
    private bool isGravityInverted = false;

    private void Awake()
    {        
        cameraManager = FindObjectOfType<CameraManager>();

        //  인게임 씬에 진입했을 때만 중력 초기화
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "MainScene")
        {
            isGravityInverted = false;
            FlipGravity(false);  // 중력 시각 & 실제 물리 적용 초기화
            Debug.Log(" 중력 초기화됨: 인게임 씬 진입 시");
        }
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

        CameraManager cam = FindObjectOfType<CameraManager>();
        if (cam != null)
            cam.FlipCameraRotation(inverted);
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

    public void ResetGravityToDefault()
    {
        isGravityInverted = false;
        FlipGravity(false); // 실제 적용
    }
}
