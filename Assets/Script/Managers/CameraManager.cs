using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform _target;
    private Vector3 _offset = new Vector3(0, 0, -10);

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    void LateUpdate()
    {
        if (_target != null)
        {
            transform.position = _target.position + _offset;
        }
    }

    // 추후 확장용 함수
    public void Shake(float intensity, float duration) { /* 카메라 흔들림 */ }
    public void Zoom(float targetSize, float duration) { /* 카메라 줌 */ }
}
