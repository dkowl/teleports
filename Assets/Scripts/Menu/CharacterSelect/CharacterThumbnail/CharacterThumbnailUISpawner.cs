using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterThumbnailUISpawner : PrefabSpawner {

    public int characterSlotID;
    public Transform parentTransform;

    public override void AfterSpawn()
    {
        CharacterThumbnailUI thumbnail = spawnedInstance.GetComponent<CharacterThumbnailUI>();

        thumbnail.SetCharacterSlotID(characterSlotID);
        spawnedInstance.gameObject.transform.SetParent(parentTransform);
        spawnedInstance.transform.SetSiblingIndex(characterSlotID);
    }
}
