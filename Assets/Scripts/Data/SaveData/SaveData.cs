﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public partial class SaveData
{
    [SerializeField] private string accountName;
    [SerializeField] private byte characterSlotLimit;
    [SerializeField] private byte currentPlayerDataID;
    [SerializeField] private PlayerData[] playerData;

    public delegate void OnCharacterIDChanged();
    public static event OnCharacterIDChanged OnCharacterIDChangedEvent;

    public SaveData()
    {
        accountName = "New account";
        characterSlotLimit = 3;
        currentPlayerDataID = 0;
        playerData = new PlayerData[characterSlotLimit];
    }

    public void DeleteCurrentPlayer()
    {
        playerData[currentPlayerDataID] = null;
        OnCharacterIDChangedEvent();
    }

    public void CreateNewPlayer(string name, string raceName)
    {
        if(CurrentPlayerData() == null)
        {
            if (MainData.Game.GetRace(raceName) != null)
            {
                playerData[currentPlayerDataID] = new PlayerData(name, raceName);
            }
            else
            {
                Debug.Log(raceName + " race does not exist");
            }
        }
        else
        {
            Debug.Log("Hero slot already taken");
        }
    }

    public PlayerData CurrentPlayerData()
    {
        return GetPlayerData(currentPlayerDataID);
    }

    public PlayerData GetPlayerData(int id)
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

    public void CorrectInvalidData()
    {
        foreach(var data in playerData)
        {
            if (data != null) data.CorrectInvalidData();
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

