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

    public delegate void OnCharacterIDChanged();
    public static event OnCharacterIDChanged OnCharacterIDChangedEvent;

    public IPlayerData currentPlayerData()
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
            Debug.Log("Changing char ID!");
            if(OnCharacterIDChangedEvent != null)
            {
                Debug.Log("Firing event!");
                OnCharacterIDChangedEvent();
            }
        }
    }
}

