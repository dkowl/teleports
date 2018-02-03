using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AiParameters {

	[SerializeField] AiType aiType;
	[SerializeField] List<SkillID> attacks;

    public AiParameters(AiParameters other)
    {
        aiType = other.aiType;
        attacks = new List<SkillID>(other.attacks);
    }

    public AiType AiType
    {
        get { return aiType; }
    }

    public List<MappedListID> AttackIds {
        get {
            if (attacks == null) return null;
            else return attacks.ConvertAll(x => (MappedListID)x);
        }
    }
}
