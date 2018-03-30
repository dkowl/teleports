using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Data/Race/Data")]
public class Race : UniqueScriptableObject {

    [FormerlySerializedAs("baseStats_")]
    [SerializeField] private UnitData baseStats;
    [SerializeField] private bool isPlayable = false;
    [SerializeField] private List<EquipmentSlotType> availableEqSlots = new List<EquipmentSlotType>();
    [SerializeField] private TextAsset description;
    [SerializeField] private RaceGraphics graphics;

    public UnitData BaseStats => baseStats;
    public bool IsPlayable => isPlayable;
    public List<EquipmentSlotType> AvailableEqSlots => availableEqSlots;
    public string Description => description.text;
    public RaceGraphics Graphics => graphics;
}
