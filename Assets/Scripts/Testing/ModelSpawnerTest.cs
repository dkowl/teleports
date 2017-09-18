using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSpawnerTest : MonoBehaviour {

    public PlayerData playerDataEditor;
    public Vector3 characterLocalPositionOffset, characterLocalRotationOffset; 
    public Vector3 teleportLocalPositionOffset, teleportLocalRotationOffset;

    private IPlayerData playerData;
    private GameObject character, teleport;

    public IPlayerData CurrentPlayerData
    {
        get
        {
            if (playerData == null)
            {
                playerData = playerDataEditor;
            }

            return playerData;
        }
    }

    public void Awake()
    {
        Race race = MainData.CurrentGameData.GetRace(CurrentPlayerData.RaceName);

        character = Instantiate(race.Graphics.modelObject, transform);
        character.GetComponentInChildren<Animator>().runtimeAnimatorController = race.Graphics.uiAnimationController;
        character.transform.localPosition += characterLocalPositionOffset;
        character.transform.Rotate(characterLocalRotationOffset, Space.Self);

        teleport = Instantiate(CurrentPlayerData.CurrentTeleportData.Graphics.modelObject, transform);
        teleport.transform.localPosition += teleportLocalPositionOffset;
        teleport.transform.Rotate(teleportLocalRotationOffset, Space.Self);
    }
}
