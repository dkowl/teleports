using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Data/Race/Data")]
public class Race : UniqueScriptableObject {

    [FormerlySerializedAs("baseStats_")]
    [SerializeField] private UnitData baseStats;
    [SerializeField] private bool isPlayable = false;
    [SerializeField] private List<EquipmentSlotType> availableEqSlots = new List<EquipmentSlotType>();
    [SerializeField] private TextAsset description;
    [SerializeField] private RaceGraphics graphics;

    private void OnValidate()
    {
        //baseStats.CorrectInvalidData();
#if UNITY_EDITOR
        //EditorUtility.SetDirty(this);
        //AssetDatabase.SaveAssets();
        //Debug.LogFormat("Validated {0}", UniqueName);
#endif
    }

    public UnitData BaseStats => baseStats;
    public bool IsPlayable => isPlayable;
    public List<EquipmentSlotType> AvailableEqSlots => availableEqSlots;
    public string Description => description.text;
    public RaceGraphics Graphics => graphics;
}
