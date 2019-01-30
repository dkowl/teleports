using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ModelSpawnerTest : LoadableBehaviour {
    
    public Vector3 characterLocalPositionOffset, characterLocalRotationOffset; 
    public Vector3 teleportLocalPositionOffset, teleportLocalRotationOffset;
    
    private GameObject character, teleport;
    private bool shouldRespawnModels = true;

    protected override void LoadDataInternal()
    {
        if (!Application.isPlaying) return;

        GameState.HeroChangedEvent -= OnModelChanged;
        GameState.HeroChangedEvent += OnModelChanged;

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

        character = UnitModelAssembler.GetModel(Main.GameState.CurrentHeroData.UnitData, true, true);
        if (character == null)
            return;
        character.transform.localPosition += characterLocalPositionOffset;
        character.transform.Rotate(characterLocalRotationOffset, Space.Self);

            
        teleport = Instantiate(CurrentHeroData.TeleportData.Graphics.modelObject, transform);
        teleport.transform.localPosition += teleportLocalPositionOffset;
        teleport.transform.Rotate(teleportLocalRotationOffset, Space.Self);

        shouldRespawnModels = false;
    }

    protected override void SubscribeInternal()
    {
        base.SubscribeInternal();
        GameState.HeroChangedEvent += OnModelChanged;
    }

    protected override void UnsubscribeInternal()
    {
        base.UnsubscribeInternal();
        GameState.HeroChangedEvent -= OnModelChanged;
    }

    public void OnModelChanged()
    {
        shouldRespawnModels = true;
        LoadDataInternal();
    }

    public HeroData CurrentHeroData => Main.GameState.CurrentHeroData;
}
