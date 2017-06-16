using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "teleportData", menuName = "Custom/TeleportData", order = 1)]
public class TeleportData : ScriptableObject {

    public float time_;
    public float Time
    {
        get { return time_; }
    }
}
