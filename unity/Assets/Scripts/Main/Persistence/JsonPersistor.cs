using System.IO;
using FullSerializer;
using UnityEngine;

public class JsonGameStatePersistor : IPersistor<GameState> {

    private static readonly string DefaultAccountName = "DefaultAccount";
    private static readonly fsSerializer Serializer = new fsSerializer();

    public void Save(GameState value)
    {
        Debug.Log("Saving...");
        fsData data;
        Serializer.TrySerialize(value, out data).AssertSuccessWithoutWarnings();

        string jsonString = fsJsonPrinter.PrettyJson(data);
        Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
        File.WriteAllText(SavePath, jsonString);
    }

    public GameState Load()
    {
        Debug.Log("Loading save...");
        if (!File.Exists(SavePath))
        {
            Debug.Log("Save file not found!");
            return new GameState(DefaultAccountName);
        }
        string jsonString = File.ReadAllText(SavePath);
        fsData data = fsJsonParser.Parse(jsonString);

        GameState result = null;
        Serializer.TryDeserialize(data, ref result).AssertSuccessWithoutWarnings();

        return result;
    }

    private string SavePath => Application.persistentDataPath + "/Saves/" + DefaultAccountName + ".json";
}
