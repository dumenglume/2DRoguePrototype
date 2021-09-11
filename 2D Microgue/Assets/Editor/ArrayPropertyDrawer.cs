using UnityEngine;
using UnityEditor;
using System.Collections;

// NOTE https://youtu.be/uoHc-Lz9Lsc

[CustomPropertyDrawer(typeof(ArrayLayout))]
public class ArrayPropertyDrawer : PropertyDrawer 
{
    float SPACE_BETWEEN_ELEMENTS = 18f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
    {
        EditorGUI.PrefixLabel(position, label);

        Rect newPosition = position;
        newPosition.y += SPACE_BETWEEN_ELEMENTS;

        SerializedProperty rows = property.FindPropertyRelative("rows"); // Find better way for this

        for (int i = 0; i < 7; i++)
        {
            SerializedProperty row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("row");

            newPosition.height = SPACE_BETWEEN_ELEMENTS;

            if (row.arraySize != 7) { row.arraySize = 7; } // TODO May not be necessary

            newPosition.width = position.width / 7;

            for (int j = 0; j < 7; j++) // TODO Find better method for this
            {
                EditorGUI.PropertyField(newPosition, row.GetArrayElementAtIndex(j), GUIContent.none);
                newPosition.x += newPosition.width;
            }
            
            newPosition.x = position.x;
            newPosition.y += SPACE_BETWEEN_ELEMENTS;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return SPACE_BETWEEN_ELEMENTS * 8;
    }
}

