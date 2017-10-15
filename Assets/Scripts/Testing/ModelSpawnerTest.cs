using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ModelSpawnerTest : LoadableBehaviour {

    public PlayerData playerDataEditor;
    public Vector3 characterLocalPositionOffset, characterLocalRotationOffset; 
    public Vector3 teleportLocalPositionOffset, teleportLocalRotationOffset;

    private IPlayerData playerData;
    private GameObject character, teleport;

    public IPlayerData CurrentPlayerData
    {
        get
        {
            return MainData.CurrentPlayerData;
        }
    }

    public override void LoadDataInternal()
    {
        if (Application.isPlaying)
        {
            if (character != null)
            {
                Destroy(character);
                character.tag = "Untagged";
            }
            character = PlayerSpawner.Spawn(new PlayerSpawnerParams(gameObject, PlayerSpawnerParams.SpawnType.UI));
            character.transform.localPosition += characterLocalPositionOffset;
            character.transform.Rotate(characterLocalRotationOffset, Space.Self);

            if (teleport != null)
            {
                Destroy(teleport);
            }
            teleport = Instantiate(CurrentPlayerData.CurrentTeleportData.Graphics.modelObject, transform);
            teleport.transform.localPosition += teleportLocalPositionOffset;
            teleport.transform.Rotate(teleportLocalRotationOffset, Space.Self);
        }
    }
}
