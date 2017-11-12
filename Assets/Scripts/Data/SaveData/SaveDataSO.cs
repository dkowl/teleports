using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "newSaveData", menuName = "Data/Save")]
public class SaveDataSO : ScriptableObject {

    public SaveData saveData;

    public delegate void OnSaveLoad();
    public static event OnSaveLoad LoadEvent;

    public void Save()
    {
        Debug.Log("Saving...");
        Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
        string jsonString = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, jsonString);
    }

    public void Load()
    {
        Debug.Log("Loading save...");
        if (!File.Exists(SavePath))
        {
            Debug.Log("Save file not found!");
            return;
        }
        string jsonString = File.ReadAllText(SavePath);
        saveData = JsonUtility.FromJson<SaveData>(jsonString);
        saveData.CorrectInvalidData();
        if(LoadEvent != null)
            LoadEvent();
    }

    private string SavePath
    {
        get
        {
            return Application.persistentDataPath + "/Saves/" + saveData.AccountName + ".json";
        }
    }
}
