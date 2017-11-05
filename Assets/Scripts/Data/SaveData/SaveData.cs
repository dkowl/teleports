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

    public IPlayerData CurrentPlayerData()
    {
        return playerData[currentPlayerDataID];
    }

    public IPlayerData GetPlayerData(int id)
    {
        if(id < playerData.Length)
        {
            return playerData[id];
        }
        else
        {
            return null;
        }
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

