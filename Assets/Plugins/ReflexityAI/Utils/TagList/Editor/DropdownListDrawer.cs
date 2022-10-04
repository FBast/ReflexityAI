using System;
using UnityEditor;
using UnityEngine;

namespace Plugins.ReflexityAI.Utils.TagList.Editor {
    [CustomPropertyDrawer(typeof(DropdownList))]
    public class DropdownListDrawer : PropertyDrawer {
        
        // Draw the property inside the given rect
        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
            if (attribute is DropdownList dropdownFromList) {
                string[] list = dropdownFromList.List;
                if (list.Length > 0) {
                    int index = Mathf.Max(0, Array.IndexOf (list, property.stringValue));
                    index = EditorGUI.Popup(position, property.displayName, index, list);
                    property.stringValue = list [index];
                }
            }
        }
        
    }
}