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
        if (index == currentIndex || index < 0 || index >= spawnedCharacters.Count)
            return;

        spawnedCharacters[currentIndex].SetActive(false);
        spawnedCharacters[index].SetActive(true);
        currentIndex = index;
    }

    public GameObject CurrentCharacter => spawnedCharacters.Count > 0 ? spawnedCharacters[currentIndex] : null;
}
