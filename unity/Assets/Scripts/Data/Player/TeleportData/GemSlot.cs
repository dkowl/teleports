using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GemSlot : UnlockableSlot {

    [SerializeField] private string gemName;
    [SerializeField] private float essence;

    public int Essence
    {
        get { return (int)essence; }
    }

	
}
