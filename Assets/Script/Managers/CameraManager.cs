using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform _target;
    private Vector3 _offset = new Vector3(0, 0, -10);
    private float _zRotation = 0f;

    void LateUpdate()
    {
        if (_target == null || !_target.gameObject.activeInHierarchy)
        {
            var character = FindCurrentCharacter();
            if (character != null)
                _target = character.transform;
        }

        if (_target != null)
        {
            transform.position = _target.position + _offset;
            transform.rotation = Quaternion.Euler(0f, 0f, _zRotation);
        }
    }

    private GameObject FindCurrentCharacter()
    {
        CharacterManager cm = FindObjectOfType<CharacterManager>();
        if (cm != null)
            return cm.CurrentCharacter;
        return null;
    }

    public void FlipCameraRotation(bool inverted)
    {
        _zRotation = inverted ? 180f : 0f;
    }
}
