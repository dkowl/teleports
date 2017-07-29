using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MainData : MonoBehaviour
{
    [CreateAssetMenu(fileName = "saveData", menuName = "Custom/SaveData", order = 6)]
    [System.Serializable]
    private partial class SaveData : ScriptableObject
    {

        private string accountName;
        private byte characterSlotLimit;
        private byte currentPlayerDataID;
        private PlayerData[] playerData;

        private PlayerData currentPlayerData()
        {
            return playerData[currentPlayerDataID];
        }
    }
}
