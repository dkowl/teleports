using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "saveData", menuName = "Custom/SaveData", order = 6)]
public class SaveData : ScriptableObject {

    public string accountName;
    public byte characterSlotLimit;
    public byte currentPlayerDataID;
    public PlayerData[] playerData;

    public PlayerData currentPlayerData()
    {
        return playerData[currentPlayerDataID];
    }
}
