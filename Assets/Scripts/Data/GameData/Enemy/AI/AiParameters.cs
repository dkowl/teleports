using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AiParameters {

	[SerializeField] AiType aiType;

    public AiType AiType
    {
        get { return aiType; }
    }
}
