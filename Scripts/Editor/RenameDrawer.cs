using UnityEngine;
using UnityEditor;

namespace ollyisonit.UnityEditorAttributes
{
    // original code from https://answers.unity.com/questions/1487864/change-a-variable-name-only-on-the-inspector.html

    [CustomPropertyDrawer(typeof(RenameAttribute))]
    public class RenameEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (GUI.enabled)
            {
                EditorGUI.PropertyField(position, property, new GUIContent((attribute as RenameAttribute).NewName), true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            bool enabled = GUI.enabled;

            if (enabled)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            else
            {
                //The property is not being drawn
                //We want to undo the spacing added before and after the property
                return -EditorGUIUtility.standardVerticalSpacing;
                //return 0.0f;
            }

        }
    }
}
