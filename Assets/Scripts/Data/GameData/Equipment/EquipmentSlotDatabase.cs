using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "equipmentSlotDatabase", menuName = "Data/Equipment/SlotDatabase")]
public class EquipmentSlotDatabase : ScriptableObject, IChangeDetectableAsset {

    private EquipmentSlotData[] data;
    private bool isInitialized = false;

    [SerializeField]
    List<EquipmentSlotData> inspectorData = new List<EquipmentSlotData>();

    public void Initialize() {

        data = new EquipmentSlotData[Enum.GetValues(typeof(EquipmentSlot)).Length];
        foreach(EquipmentSlotData eqData in inspectorData)
        {
            data[(int)eqData.SlotType] = eqData;
        }

        Debug.Log("EquipmentSlotDatabase initialized!");
        isInitialized = true;
    }

    public void OnWillSaveAssets()
    {
        Initialize();
    }

    public EquipmentSlotData GetSlotData(EquipmentSlot slotType)
    {
        if (!isInitialized) Initialize();

        return data[(int)slotType];
    }

}
