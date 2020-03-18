using System;
using UnityEditor;
using UnityEngine;

namespace Plugins.xNodeUtilityAi.Utils.TagList.Editor {
    [CustomPropertyDrawer(typeof(TagListProperty))]
    public class TagListDrawer : PropertyDrawer {
        
        // Draw the property inside the given rect
        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
            if (attribute is TagListProperty tagListProperty) {
                string[] list = tagListProperty.List;
                if (list.Length > 0) {
                    int index = Mathf.Max(0, Array.IndexOf (list, property.stringValue));
                    index = EditorGUI.Popup(position, property.displayName, index, list);
                    property.stringValue = list [index];
                } else {
                    EditorGUI.HelpBox(position, "Open the AIBrainGraph to add Tags", MessageType.Warning);
                }
            }
        }
        
    }
}