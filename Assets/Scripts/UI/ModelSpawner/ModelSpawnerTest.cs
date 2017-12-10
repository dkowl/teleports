using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ModelSpawnerTest : LoadableBehaviour {
    
    public Vector3 characterLocalPositionOffset, characterLocalRotationOffset; 
    public Vector3 teleportLocalPositionOffset, teleportLocalRotationOffset;
    
    private GameObject character, teleport;
    private bool shouldRespawnModels = true;

    override protected void LoadDataInternal()
    {
        if (Application.isPlaying)
        {
            SaveData.OnCharacterIDChangedEvent -= OnModelChanged;
            SaveData.OnCharacterIDChangedEvent += OnModelChanged;

            if (character != null)
            {
                if (shouldRespawnModels)
                {
                    Destroy(character);
                    character.tag = "Untagged";
                }
                else return;
            }
            if (teleport != null)
            {
                Destroy(teleport);
            }
            character = PlayerSpawner.Spawn(new PlayerSpawnerParams(gameObject, PlayerSpawnerParams.SpawnType.UI));
            if (character == null)
                return;
            character.transform.localPosition += characterLocalPositionOffset;
            character.transform.Rotate(characterLocalRotationOffset, Space.Self);

            
            teleport = Instantiate(CurrentPlayerData.CurrentTeleportData.Graphics.modelObject, transform);
            teleport.transform.localPosition += teleportLocalPositionOffset;
            teleport.transform.Rotate(teleportLocalRotationOffset, Space.Self);

            shouldRespawnModels = false;
        }
    }

    protected override void SubscribeInternal()
    {
        base.SubscribeInternal();
        SaveData.OnCharacterIDChangedEvent += OnModelChanged;
    }

    protected override void UnsubscribeInternal()
    {
        base.UnsubscribeInternal();
        SaveData.OnCharacterIDChangedEvent -= OnModelChanged;
    }

    public void OnModelChanged()
    {
        shouldRespawnModels = true;
        LoadDataInternal();
    }

    public IPlayerData CurrentPlayerData
    {
        get
        {
            return MainData.CurrentPlayerData;
        }
    }
}
