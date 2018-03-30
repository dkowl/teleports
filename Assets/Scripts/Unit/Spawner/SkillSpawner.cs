using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillSpawner  {

    public static Skill SpawnSkill(GameObject gameObject, SkillData skillData, Unit unit)
    {
        GameObject skillGameObject = new GameObject(skillData.UniqueName);
        skillGameObject.transform.parent = gameObject.transform;
        Skill skillComponent;
        switch (skillData.SkillType)
        {
            case SkillType.Attack:
                skillComponent = skillGameObject.AddComponent<Attack>();
                break;
            case SkillType.Combo:
                skillComponent = skillGameObject.AddComponent<Combo>();
                break;
            default:
                skillComponent = null;
                break;
        }

        if (skillComponent != null)
        {
            skillComponent.Initialize(skillData, unit);
        }
        return skillComponent;
    }
}
