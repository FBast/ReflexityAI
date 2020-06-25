using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Plugins.ReflexityAI.MainNodes.Editor {
    [CustomNodeEditor(typeof(UtilityNode))]
    public class UtilityNodeEditor : NodeEditor {

        private UtilityNode _utilityNode;

        public override void OnBodyGUI() {
            if (_utilityNode == null) _utilityNode = (UtilityNode) target;
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_utilityNode.MinX)));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_utilityNode.X)));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_utilityNode.MaxX)));
            _utilityNode.MaxY = EditorGUILayout.IntField("Max Rank", _utilityNode.MaxY);
            EditorGUILayout.CurveField(GUIContent.none, _utilityNode.Function, GUILayout.Height(50));
            NodeEditorGUILayout.AddPortField(_utilityNode.GetPort(nameof(_utilityNode.Rank)));
            _utilityNode.MinY = EditorGUILayout.IntField("Min Rank", _utilityNode.MinY);
        }

    }
}
