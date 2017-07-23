using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankPointsUI : MonoBehaviour {

    public Text text_;

    void Start()
    {
        text_.text = "Rank points: " + MainData.RankPoints.ToString();
    }
}
