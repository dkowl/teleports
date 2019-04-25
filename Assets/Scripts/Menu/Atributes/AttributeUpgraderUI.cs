using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using Callback = System.Action<UnitAttributesData.AttributeType>;

[ExecuteInEditMode]
public class AttributeUpgraderUI : MonoBehaviour
{
    [SerializeField] private UnitAttributesData.AttributeType attributeType;
    [SerializeField] private int attributeValue;
    private Callback plusCallback;
    private Callback minusCallback;

    [SerializeField] private Text nameText;
    [SerializeField] private Text valueText;
    [SerializeField] private Slider valueSlider;

    private void Update()
    {
        nameText.text = UnitAttributeData.GetName(attributeType);
        valueText.text = attributeValue.ToString();
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

    public UnitAttributesData.AttributeType AttributeType { set { attributeType = value; } }
    public int AttributeValue { set { attributeValue = value; } }
    public Callback PlusCallback { set { plusCallback = value; } }
    public Callback MinusCallback { set { minusCallback = value; } }
}
