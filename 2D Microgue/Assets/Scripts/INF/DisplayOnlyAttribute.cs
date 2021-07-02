﻿using UnityEngine;
using UnityEditor;

public class DisplayOnlyAttribute : PropertyAttribute
{
    public DisplayOnlyAttribute()
    {
    }
}

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DisplayOnlyAttribute))]
    public class DisplayOnlyAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
#endif
