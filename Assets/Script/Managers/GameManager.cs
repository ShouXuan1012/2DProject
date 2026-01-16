using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        // Managers 초기화 강제 호출
        _ = Managers.Input;
        _ = Managers.Character;
        _ = Managers.Pool;
        _ = Managers.Gravity;         
        _ = Managers.UI;
        _ = Managers.Time;
        _ = Managers.GameSave;
    }
}
