using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TextMeshProUGUI;
using UnityEditor;

[ExecuteInEditMode]
public class TextWidget : MonoBehaviour
{
    private Text text;
    [SerializeField] private List<ITextWidgetStyle> styles;

    void Awake()
    {
        if(text == null)
        {
            text = gameObject.AddComponent<Text>();
        }
    }

    void ApplyStyles()
    {
        foreach(var style in styles)
        {
            ApplyStyle(style);
        }
    }

    private void ApplyStyle(ITextWidgetStyle style)
    {
        text.font = style.Font ?? text.font;
    }

    [MenuItem("GameObject/Widgets/Text", priority = 11)]
    private static void Create(UnityEditor.MenuCommand command)
    {
        GameObject widget = new GameObject("Text Widget");
        widget.AddComponent<TextWidget>();
        widget.CreateFromEditor(command);
    }
}
