using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameSaveManager : MonoBehaviour
{
    public void SaveGame()
    {
        SaveData data = new SaveData();

        // 현재 씬 이름
        data.currentScene = SceneManager.GetActiveScene().name;

        // 플레이어 위치
        data.playerPosition = Managers.Character.CurrentCharacter.transform.position;

        // 중력 상태
        data.isGravityInverted = Managers.Gravity.GetGravityState();

        // 캐릭터 생존 여부
        var charList = Managers.Character.CurrentCharacterList;
        data.characterIsDead = new bool[charList.Count];
        for (int i = 0; i < charList.Count; i++)
        {
            var ctrl = charList[i].GetComponent<BaseCharacterController>();
            data.characterIsDead[i] = ctrl.isDead;
        }

        SaveSystem.Save(data);
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.Load();
        if (data == null)
            return;

        // 씬 로드 후 적용
        StartCoroutine(ApplyAfterSceneLoad(data));
    }

    private IEnumerator ApplyAfterSceneLoad(SaveData data)
    {
        SceneManager.LoadScene(data.currentScene);
        yield return new WaitForSeconds(0.1f); // 씬 로드 기다리기

        // 위치 복원
        Managers.Character.CurrentCharacter.transform.position = data.playerPosition;

        // 중력 복원
        Managers.Gravity.FlipGravity(data.isGravityInverted);

        // 캐릭터 상태 복원
        var charList = Managers.Character.CurrentCharacterList;
        for (int i = 0; i < charList.Count; i++)
        {
            var ctrl = charList[i].GetComponent<BaseCharacterController>();
            ctrl.SetDeadState(data.characterIsDead[i]);
        }
    }
}
