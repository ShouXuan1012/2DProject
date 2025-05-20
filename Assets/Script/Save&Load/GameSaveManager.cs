using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameSaveManager : MonoBehaviour
{
    public void SaveGame(int slot)
    {
        SaveData data = new SaveData();
        data.currentScene = SceneManager.GetActiveScene().name;
        data.playerPosition = Managers.Character.CurrentCharacter.transform.position;
        data.isGravityInverted = Managers.Gravity.GetGravityState();

        var charList = Managers.Character.CurrentCharacterList;
        data.characterIsDead = new bool[charList.Count];
        for (int i = 0; i < charList.Count; i++)
        {
            var ctrl = charList[i].GetComponent<BaseCharacterController>();
            data.characterIsDead[i] = ctrl.isDead;
        }

        SaveSystem.Save(data, slot);
    }

    public void LoadGame(int slot)
    {
        SaveData data = SaveSystem.Load(slot);
        if (data == null)
            return;

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
