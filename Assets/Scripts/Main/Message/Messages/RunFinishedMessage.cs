using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunFinishedMessage : IMessage{

    int score;

    public RunFinishedMessage(int score)
    {
        this.score = score;
    }

    public int Score
    {
        get{ return score; }
    }
}
