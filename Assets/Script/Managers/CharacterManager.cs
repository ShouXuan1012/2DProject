using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private List<GameObject> spawnedCharacters = new();
    private int currentIndex = 0;

    public void Init(List<GameObject> characterPrefabs)
    {
        foreach (GameObject prefab in characterPrefabs)
        {
            GameObject instance = Instantiate(prefab);
            instance.SetActive(false);
            spawnedCharacters.Add(instance);
        }

        spawnedCharacters[currentIndex].SetActive(true);
    }

    public void ChangeCharacter(int index)
    {
        if (index == currentIndex || index < 0 || index >= spawnedCharacters.Count || spawnedCharacters[index].GetComponent<BaseCharacterController>().isDead)
            return;
        Vector2 pos = spawnedCharacters[currentIndex].transform.position;

        // 1. 현재 위치 기억
        Vector3 currentPos = spawnedCharacters[currentIndex].transform.position;

        // 2. 현재 캐릭터 비활성화
        spawnedCharacters[currentIndex].SetActive(false);
        spawnedCharacters[index].transform.position = pos;

        // 3. 새 캐릭터 위치 설정 + 활성화
        spawnedCharacters[index].transform.position = currentPos;
        spawnedCharacters[index].SetActive(true);

        // 4. 인덱스 갱신
        currentIndex = index;
    }


    public GameObject CurrentCharacter => spawnedCharacters.Count > 0 ? spawnedCharacters[currentIndex] : null;
}
