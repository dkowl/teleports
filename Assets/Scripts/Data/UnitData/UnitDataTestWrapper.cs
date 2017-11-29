using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "test", menuName = "Data/Unit/TestWrapper")]
public class UnitDataTestWrapper : SerializedScriptableObject {

    public UnitData unitData;
}
