using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

//Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)
//Modified by: -

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        
        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);        

        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;
        if (enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }        

        GUI.enabled = wasEnabled;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

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


        /*
        //Get the base height when not expanded
        var height = base.GetPropertyHeight(property, label);

        // if the property is expanded go through all its children and get their height
        if (property.isExpanded)
        {
            var propEnum = property.GetEnumerator();
            while (propEnum.MoveNext())
                height += EditorGUI.GetPropertyHeight((SerializedProperty)propEnum.Current, GUIContent.none, true);
        }
        return height;*/
    }

    private bool GetConditionalHideAttributeResult(ConditionalHideAttribute condHAtt, SerializedProperty property)
    {
        bool enabled;
        switch (condHAtt.foldBehavior)
        {
            case (ConditionalHideAttribute.FoldBehavior.And):
     
                enabled = true;
                break;
            case (ConditionalHideAttribute.FoldBehavior.Or):

                enabled = false;
                break;
            default:
                throw new NotImplementedException("Case not found for FoldBehavior " + condHAtt.foldBehavior);
        }


        foreach ((string field, object comp) pair in condHAtt.conditions)
        {
            enabled = condHAtt.Combine(enabled, CheckPropertyType(GetFieldFromProperty(property, pair.field), pair.comp));
        }

        return enabled;
    }


    private SerializedProperty GetFieldFromProperty(SerializedProperty property, string field)
    {
        //Handle primary property
        SerializedProperty sourcePropertyValue = null;
        //Get the full relative property path of the sourcefield so we can have nested hiding.Use old method when dealing with arrays
        if (!property.isArray)
        {
            string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
            Regex replaceSearch = new Regex(property.name + "$");
            string conditionPath = replaceSearch.Replace(propertyPath, (Match m) => field);
            sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

            //if the find failed->fall back to the old system
            if (sourcePropertyValue == null)
            {
                //original implementation (doens't work with nested serializedObjects)
                sourcePropertyValue = property.serializedObject.FindProperty(field);
            }
        }
        else
        {
            //original implementation (doens't work with nested serializedObjects)
            sourcePropertyValue = property.serializedObject.FindProperty(field);
        }

        return sourcePropertyValue;
    }


    private bool CheckPropertyType(SerializedProperty sourcePropertyValue, object comparison)
    {
        //Note: add others for custom handling if desired
        switch (sourcePropertyValue.propertyType)
        {                
            case SerializedPropertyType.Boolean:
                return sourcePropertyValue.boolValue;                
            case SerializedPropertyType.ObjectReference:
                return sourcePropertyValue.objectReferenceValue != null;
            case SerializedPropertyType.Enum:
                return sourcePropertyValue.enumValueIndex == (int)comparison;
            default:
                Debug.LogError("Data type of the property used for conditional hiding [" + sourcePropertyValue.propertyType + "] is currently not supported");
                return true;
        }
    }
}
