using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using Callback = System.Action<UnitAttributesData.AttributeType>;
using System.Text;

[ExecuteInEditMode]
public class AttributeUpgraderUI : MonoBehaviour
{
    [SerializeField] private UnitAttributesData.AttributeType attributeType;
    [SerializeField] private int attributeValue;
    private string extraValueColor;
    private Callback plusCallback;
    private Callback minusCallback;

    [SerializeField] private Text nameText;
    [SerializeField] private Text valueText;
    [SerializeField] private Slider valueSlider;

    private void Update()
    {
        nameText.text = UnitAttributeData.GetName(attributeType);
        valueText.text = GetValueString();
        valueSlider.value = (float)attributeValue / 100.0f;
    }

    public void OnPlusClick()
    {
        plusCallback(attributeType);
    }

    public void OnMinusClick()
    {
        minusCallback(attributeType);
    }

    private string GetValueString()
    {
        var builder = new StringBuilder();
        if (extraValueColor != null)
        {
            builder.Append($"<color=#{extraValueColor}>");
        }
        builder.Append(attributeValue.ToString());
        if (extraValueColor != null)
        {
            builder.Append("</color>");
        }
        return builder.ToString();
    }

    public UnitAttributesData.AttributeType AttributeType { set { attributeType = value; } }
    public int AttributeValue { set { attributeValue = value; } }
    public string ExtraValueColor {  set { extraValueColor = value; } }
    public Callback PlusCallback { set { plusCallback = value; } }
    public Callback MinusCallback { set { minusCallback = value; } }
}
