using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameSaveManager : MonoBehaviour
{
    public void SaveGame(int slot)
    {        
        SaveData data = new SaveData();
        data.currentScene = SceneManager.GetActiveScene().name;
        data.playerPosition = Managers.Character.CurrentCharacter.transform.position;
        data.isGravityInverted = Managers.Gravity.GetGravityState();
        data.currentCharacterIndex = Managers.Character.CurrentIndex;

        var charList = Managers.Character.CurrentCharacterList;
        data.characterIsDead = new bool[charList.Count];
        for (int i = 0; i < charList.Count; i++)
        {
            var ctrl = charList[i].GetComponent<BaseCharacterController>();
            data.characterIsDead[i] = ctrl.isDead;
        }

        //오브젝트 상태 저장(몬스터, 아이템, 파괴가능 오브젝트)
        string[] tags = { "Enemy", "Potion", "BreakableWall" };
        data.aliveObjectIDs = new List<string>();

        foreach (string tag in tags)
        {
            foreach(var obj in GameObject.FindGameObjectsWithTag(tag))
            {
                var id = obj.GetComponent<UniqueID>();
                if (id != null && !data.aliveObjectIDs.Contains(id.id))
                    Destroy(obj);
            }
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
        Managers.Character.CurrentCharacterList[data.currentCharacterIndex].transform.position = data.playerPosition;

        // 중력 복원
        Managers.Gravity.FlipGravity(data.isGravityInverted);

        // 캐릭터 상태 복원
        var charList = Managers.Character.CurrentCharacterList;

        for (int i = 0; i < charList.Count; i++)
        {
            var ctrl = charList[i].GetComponent<BaseCharacterController>();
            ctrl.SetDeadState(data.characterIsDead[i]);

            // 현재 캐릭터만 활성화
            charList[i].SetActive(i == data.currentCharacterIndex && !ctrl.isDead);
        }

        //오브젝트 상태 로딩(몬스터, 아이템, 파괴가능 오브젝트)
        string[] tags = { "Enemy", "Potion", "BreakableWall" };
        data.aliveObjectIDs = new List<string>();

        foreach (string tag in tags)
        {
            foreach (var obj in GameObject.FindGameObjectsWithTag(tag))
            {
                var id = obj.GetComponent<UniqueID>();
                if (id != null && !data.aliveObjectIDs.Contains(id.id))
                    Destroy(obj);
            }
        }
    }
}
