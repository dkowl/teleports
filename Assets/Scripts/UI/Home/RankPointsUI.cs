using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankPointsUI : MonoBehaviour {

    public Text text;

    private void Start()
    {
        text.text = "Rank points: " + Main.GameState.CurrentHeroData.RankPoints;
    }
}
