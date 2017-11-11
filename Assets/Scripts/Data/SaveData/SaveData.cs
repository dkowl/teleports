using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public partial class SaveData
{
    [SerializeField]
    private string accountName;
    [SerializeField]
    private byte characterSlotLimit;
    [SerializeField]
    private byte currentPlayerDataID;
    [SerializeField]
    private PlayerData[] playerData;

    public delegate void OnCharacterIDChanged();
    public static event OnCharacterIDChanged OnCharacterIDChangedEvent;

    public void DeleteCurrentPlayer()
    {
        playerData[currentPlayerDataID] = null;
        OnCharacterIDChangedEvent();
    }

    public IPlayerData CurrentPlayerData()
    {
        if (playerData[currentPlayerDataID].CharacterName == "")
            return null;
        return playerData[currentPlayerDataID];
    }

    public IPlayerData GetPlayerData(int id)
    {
        if(id < playerData.Length && playerData[id].CharacterName != "")
        {
            return playerData[id];
        }
        else
        {
            return null;
        }
    }

    public void CorrectInvalidData()
    {
        foreach(var data in playerData)
        {
            data.CorrectInvalidData();
        }
    }

    public string AccountName
    {
        get { return accountName; }
    }

    public int CharacterSlotLimit
    {
        get { return characterSlotLimit; }
    }

    public int CurrentSlotID
    {
        get { return currentPlayerDataID; }
        set {
            currentPlayerDataID = (byte)value;
            if(OnCharacterIDChangedEvent != null)
            {
                OnCharacterIDChangedEvent();
            }
        }
    }
}

