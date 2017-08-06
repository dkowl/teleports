using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "saveData", menuName = "Custom/SaveData", order = 6)]
[System.Serializable]
public partial class SaveData : ScriptableObject
{
    [SerializeField]
    private string accountName;
    [SerializeField]
    private byte characterSlotLimit;
    [SerializeField]
    private byte currentPlayerDataID;
    [SerializeField]
    private PlayerData[] playerData;

    public IPlayerData currentPlayerData()
    {
        return playerData[currentPlayerDataID];
    }
}

