using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class AttributesMenu : MonoBehaviour {

	[SerializeField] private List<UnitAttributesData.AttributeType> attributeTypes;
    [SerializeField] private TextWidget titleText;

    private AttributeUpgraderUI[] upgraders;
    private int[] spentAttributePoints;
    private int unspentAttributePoints;
    private static readonly string ValueHighlightColor = "fd6";

    void Start()
    {
        upgraders = GetComponentsInChildren<AttributeUpgraderUI>();
        ResetState();
    }

    private void ResetState()
    {
        spentAttributePoints = new int[3];
        unspentAttributePoints = Main.GameState.CurrentHeroData.TotalAttributePoints;
        Update();
    }

    void Update()
    {
        Debug.Assert(attributeTypes.Count == upgraders.Length);
        for(int i = 0; i < attributeTypes.Count; i++)
        {
            titleText.Text = GetTitleText();

            upgraders[i].AttributeType = attributeTypes[i];
            int baseValue = (int)Main.GameState.CurrentHeroData.UnitData.Attributes.GetAttribute(attributeTypes[i]).Value;
            int extraValue = spentAttributePoints[i];
            if (extraValue > 0) upgraders[i].ExtraValueColor = ValueHighlightColor;
            else upgraders[i].ExtraValueColor = null;
            upgraders[i].AttributeValue = baseValue + extraValue;
            upgraders[i].PlusCallback = SpendAttributePoint;
            upgraders[i].MinusCallback = UnspendAttributePoint;
        }
    }

    public bool HasPointsToApply()
    {
        foreach(var points in spentAttributePoints)
        {
            if (points > 0) return true;
        }
        return false;
    }

    public string GetApplyQuestionString()
    {
        var builder = new StringBuilder();
        builder.Append("Apply\n");
        for(int i = 0; i < attributeTypes.Count; i++)
        {
            if(spentAttributePoints[i] > 0)
            {
                builder.Append(UnitAttributeData.GetName(attributeTypes[i]));
                builder.Append($" +{spentAttributePoints[i].ToString()}\n");
            }
        }
        builder.Append("?");
        return builder.ToString();
    }

    public void ApplySpentPoints()
    {
        var tuples = new List<System.Tuple<UnitAttributesData.AttributeType, int>>();
        for (int i = 0; i < attributeTypes.Count; i++)
        {
            if (spentAttributePoints[i] > 0)
            {
                tuples.Add(new System.Tuple<UnitAttributesData.AttributeType, int>(attributeTypes[i], spentAttributePoints[i]));
            }
        }
        var result = Main.GameState.CurrentHeroData.ApplyAttributePoints(tuples);
        Debug.Assert(result == HeroData.ApplyAttributePointsResult.OK);
        ResetState();
    }

    private void SpendAttributePoint(UnitAttributesData.AttributeType type)
    {
        if (unspentAttributePoints == 0) return;
        int id = GetIdOfAttributeType(type);
        spentAttributePoints[id]++;
        unspentAttributePoints--;
    }

    private void UnspendAttributePoint(UnitAttributesData.AttributeType type)
    {
        int id = GetIdOfAttributeType(type);
        if (spentAttributePoints[id] == 0) return;
        spentAttributePoints[id]--;
        unspentAttributePoints++;
    }

    private int GetIdOfAttributeType(UnitAttributesData.AttributeType type)
    {
        for (int i = 0; i < attributeTypes.Count; i++)
        {
            if (attributeTypes[i] == type) return i;
        }
        Debug.Assert(false);
        return -1;
    }

    private string GetTitleText()
    {
        var builder = new StringBuilder();
        if(unspentAttributePoints > 0)
        {
            builder.Append($"<color=#{ValueHighlightColor}>");
        }
        builder.Append(unspentAttributePoints.ToString());
        if(unspentAttributePoints > 0)
        {
            builder.Append("</color>");
        }
        return builder.ToString();
    }
}
