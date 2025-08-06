using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static readonly string SavePath = Application.persistentDataPath + "/save.json";

    public static void SaveGame()
    {
        var allItems = GameObject.FindObjectsOfType<InspectableItem>();
        GameSaveData saveData = new GameSaveData();

        foreach (var item in allItems)
        {
            saveData.savedItems.Add(item.ToSaveData());
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Game Saved to " + SavePath);
    }

    public static void LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("Save file not found.");
            return;
        }

        string json = File.ReadAllText(SavePath);
        GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);

        var allItems = GameObject.FindObjectsOfType<InspectableItem>();

        foreach (var item in allItems)
        {
            var match = saveData.savedItems.Find(x => x.itemName == item.instance.data.itemName);
            if (match != null)
            {
                item.ApplySaveData(match);
            }
        }

        Debug.Log("Game Loaded from " + SavePath);
    }

    public static void DeleteSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Save deleted.");
        }
    }
}
