using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string currentScene;
    public Vector3 playerPosition;
    public bool[] characterIsDead;
    public bool isGravityInverted;
    public int currentCharacterIndex;

    public List<string> aliveObjectIDs = new();
}
