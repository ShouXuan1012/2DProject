using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string GetSlotPath(int slot) => Application.persistentDataPath + $"/save_slot{slot}.json";

    public static void Save(SaveData data, int slot)
    {
        string path = GetSlotPath(slot);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
        Debug.Log($"슬롯 {slot} 저장 완료: {path}");
    }

    public static SaveData Load(int slot)
    {
        string path = GetSlotPath(slot);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }
        Debug.LogWarning($"슬롯 {slot}에 저장된 파일이 없습니다.");
        return null;
    }

    public static bool HasSave(int slot)
    {
        return File.Exists(GetSlotPath(slot));
    }

    public static void Delete(int slot)
    {
        string path = GetSlotPath(slot);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"슬롯 {slot} 저장 파일 삭제됨");
        }
    }
}
