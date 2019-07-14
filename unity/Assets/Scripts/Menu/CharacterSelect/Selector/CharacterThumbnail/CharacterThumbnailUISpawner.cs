using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterThumbnailUISpawner : PrefabSpawner {

    public int characterSlotID;
    public Transform parentTransform;

    protected override void AfterSpawn()
    {
        CharacterThumbnailUI thumbnail = SpawnedInstance.GetComponent<CharacterThumbnailUI>();

        thumbnail.SetCharacterSlotID(characterSlotID);
        SpawnedInstance.gameObject.transform.SetParent(parentTransform);
        SpawnedInstance.transform.SetSiblingIndex(characterSlotID);
    }
}
