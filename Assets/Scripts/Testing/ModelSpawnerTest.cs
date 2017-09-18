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
            Debug.Log("Loading ze data");
            Race race = MainData.CurrentGameData.GetRace(CurrentPlayerData.RaceName);

            if (character != null)
            {
                Destroy(character);
            }
            character = Instantiate(race.Graphics.modelObject, transform);
            character.GetComponentInChildren<Animator>().runtimeAnimatorController = race.Graphics.uiAnimationController;
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
