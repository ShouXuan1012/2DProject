using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath => Application.persistentDataPath + "/save.json";

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SavePath, json);
        Debug.Log("저장 완료: " + SavePath);
    }

    public static SaveData Load()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("불러오기 완료");
            return data;
        }
        else
        {
            Debug.LogWarning("저장 파일이 없습니다.");
            return null;
        }
    }

    public static void Delete()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("저장 파일 삭제됨");
        }
    }
}
