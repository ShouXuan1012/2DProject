using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private List<GameObject> spawnedCharacters = new();
    private int currentIndex = 0;

    public int CurrentIndex => currentIndex;    

    public void Init(List<GameObject> characterPrefabs)
    {
        foreach (GameObject prefab in characterPrefabs)
        {
            GameObject instance = Instantiate(prefab);
            DontDestroyOnLoad(instance);
            instance.SetActive(false);
            spawnedCharacters.Add(instance);
        }

        spawnedCharacters[currentIndex].SetActive(true);

        Sprite skillSprite = Resources.Load<Sprite>($"Sprites/UI/{currentIndex}Skill");

        var controller = spawnedCharacters[currentIndex];
        float remaining = Managers.Time.GetRemaining(controller);
        float total = Managers.Time.GetDuration(controller);

        Managers.UI.InitSkillIcon(skillSprite, remaining, total);
    }

    public void ChangeCharacter(int index)
    {
        if (index == currentIndex || index < 0 || index >= spawnedCharacters.Count || spawnedCharacters[index].GetComponent<BaseCharacterController>().isDead)
            return;
        Vector2 pos = spawnedCharacters[currentIndex].transform.position;

        // 1. 현재 위치 기억
        Vector3 currentPos = spawnedCharacters[currentIndex].transform.position;
        if(Managers.Gravity.GetGravityState())
        {
            currentPos.y -= 0.25f;
        }
        else
        {
            currentPos.y += 0.2f;
        }

        BaseCharacterController currentChar = spawnedCharacters[currentIndex].GetComponent<BaseCharacterController>();
        if (!currentChar.CanExitNarrowSpace())
        {
            Managers.UI.ShowChangeFail();
            return;
        }
       

        // 2. 현재 캐릭터 비활성화
        spawnedCharacters[currentIndex].SetActive(false);
        spawnedCharacters[index].transform.position = pos;

        // 3. 새 캐릭터 위치 설정 + 활성화
        spawnedCharacters[index].transform.position = currentPos;
        spawnedCharacters[index].SetActive(true);       

        // 4. 인덱스 갱신
        currentIndex = index;

        // 5. 체력 UI 갱신
        int maxHp = spawnedCharacters[index].GetComponent<BaseCharacterController>().MaxHp;
        Managers.UI.InitHearts(maxHp);
        int currentHp = spawnedCharacters[index].GetComponent<BaseCharacterController>().CurrentHp;
        Managers.UI.UpdateHealth(currentHp);

        //6. 스킬 갱신      
        Sprite skillSprite = Resources.Load<Sprite>($"Sprites/UI/{index}Skill");
                
        var controller = spawnedCharacters[index];
        float remaining = Managers.Time.GetRemaining(controller);
        float total = Managers.Time.GetDuration(controller);

        Managers.UI.InitSkillIcon(skillSprite, remaining, total);
    }

    public GameObject CurrentCharacter => spawnedCharacters.Count > 0 ? spawnedCharacters[currentIndex] : null;  
}
