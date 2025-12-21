// ============================================================================
// SAVE/LOAD SYSTEM
// ============================================================================
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "savedata.json");

    public static void Save<T>(T data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    public static T Load<T>() where T : new()
    {
        if (!File.Exists(SavePath))
            return new T();

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<T>(json);
    }

    public static bool SaveExists()
    {
        return File.Exists(SavePath);
    }

    public static void DeleteSave()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }
}
