using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class RewardPopupUI : MonoBehaviour {

    [SerializeField] private TextWidget textWidget;
    [SerializeField] private Button button;
    [SerializeField] private float smallTextSize = 60;

    private void Awake()
    {
        Debug.Assert(textWidget != null);
        Debug.Assert(button != null);
    }

    public void SetClickCallback(System.Action callback)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => callback());
    }

    public void SetMainText(string text)
    {
        textWidget.Text = text;
    }

    public void AddSmallText(string text)
    {
        var builder = new StringBuilder();
        builder.Append(textWidget.Text);
        builder.Append($"\n<size={smallTextSize}>");
        builder.Append(text);
        builder.Append("</size>");
        textWidget.Text = builder.ToString();
    }
}
