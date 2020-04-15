using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Plugins.xNodeUtilityAi.MainNodes.Editor {
    [CustomNodeEditor(typeof(UtilityNode))]
    public class UtilityNodeEditor : NodeEditor {

        private UtilityNode _utilityNode;

        public override void OnBodyGUI() {
            if (_utilityNode == null) _utilityNode = (UtilityNode) target;
            NodeEditorGUILayout.PortField(_utilityNode.GetPort(nameof(_utilityNode.MinX)));
            NodeEditorGUILayout.PortField(_utilityNode.GetPort(nameof(_utilityNode.X)));
            NodeEditorGUILayout.PortField(_utilityNode.GetPort(nameof(_utilityNode.MaxX)));
            EditorGUILayout.CurveField(GUIContent.none, _utilityNode.Function, GUILayout.Height(50));
            NodeEditorGUILayout.AddPortField(_utilityNode.GetPort(nameof(_utilityNode.UtilityY)));
//            EditorGUILayout.CurveField("Function Curve", _utilityNode.Function, Color.black, Rect.zero);
        }

    }
}
