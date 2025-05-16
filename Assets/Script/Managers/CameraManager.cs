using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform _target;
    private Vector3 _offset = new Vector3(0, 0, -10);
    private float _zRotation = 0f;

    void Awake()
    {
        AttachMainCamera();
    }

    private void AttachMainCamera()
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            Transform camTransform = cam.transform;
            camTransform.SetParent(this.transform);
            camTransform.localPosition = Vector3.zero; // 원하는 오프셋 적용 가능
            camTransform.localRotation = Quaternion.identity;           
        }
        
    }

    void LateUpdate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            _target = player.transform;

        if (_target != null)
        {
            transform.position = _target.position + _offset;
            transform.rotation = Quaternion.Euler(0f, 0f, _zRotation);           
        }
    }

    public void FlipCameraRotation(bool inverted)
    {
        _zRotation = inverted ? 180f : 0f;        
    }
}
