using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(UnitAttribute))]
public class AttributeDrawer : PropertyDrawer { 

	override public void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        //Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Keyboard), label);

        //Set indent to zero
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        //Calculate rects
        Rect rawRect = new Rect(position.x, position.y, 80, position.height);
        Rect bonusRect = new Rect(position.x+ 85, position.y, 25, position.height);
        Rect multiplierRect = new Rect(position.x + 115, position.y, 25, position.height);

        //Draw fields
        EditorGUI.PropertyField(rawRect, property.FindPropertyRelative("raw"), GUIContent.none);
        EditorGUI.PropertyField(bonusRect, property.FindPropertyRelative("bonus"), GUIContent.none);
        EditorGUI.PropertyField(multiplierRect, property.FindPropertyRelative("multiplier"), GUIContent.none);

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
