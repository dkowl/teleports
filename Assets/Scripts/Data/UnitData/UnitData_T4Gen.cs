//DO NOT EDIT - Autogenerated file
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Teleports.Utils;

[System.Serializable]
public partial class UnitData : IDeepCopyable {

	[SerializeField] private string unitName;
	[SerializeField] private string raceName;
	[SerializeField] private int level;
	[SerializeField] private UnitAttributesData attributes;
	[SerializeField] private SkillID mainAttack;
	[SerializeField] private List<PerkID> perks;
	[SerializeField] private List<SkillID> skills;
	[SerializeField] private InventoryData inventory;
	[SerializeField] private bool usesRootMotion;
	[SerializeField] private bool isInitialized;


	public UnitData(UnitData other){
		unitName = other.unitName;
		raceName = other.raceName;
		level = other.level;
		attributes = new UnitAttributesData(other.attributes);
		mainAttack = new SkillID(other.mainAttack);
		perks = new List<PerkID>(other.perks);
		skills = new List<SkillID>(other.skills);
		inventory = new InventoryData(other.inventory);
		usesRootMotion = other.usesRootMotion;
		isInitialized = other.isInitialized;
	}

	public object DeepCopy(){
		return new UnitData(this);
	}



	public string UnitName => unitName;
	public string RaceName => raceName;
	public int Level => level;
	public UnitAttributesData Attributes => attributes;
	public SkillID MainAttack => mainAttack;
	public List<PerkID> Perks => perks;
	public List<SkillID> Skills => skills;
	public InventoryData Inventory => inventory;
	public bool UsesRootMotion => usesRootMotion;
	public bool IsInitialized => isInitialized;


}
	