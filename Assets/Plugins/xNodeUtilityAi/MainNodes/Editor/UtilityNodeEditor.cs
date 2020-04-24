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
//            EditorGUILayout.CurveField(GUIContent.none, _utilityNode.ScaledFunction, GUILayout.Height(50));
            NodeEditorGUILayout.AddPortField(_utilityNode.GetPort(nameof(_utilityNode.Rank)));
//            _utilityNode.MinY = EditorGUILayout.IntField("Min Rank", _utilityNode.MinY);
//            _utilityNode.MaxY = EditorGUILayout.IntField("Max Rank", _utilityNode.MaxY);
//            Keyframe[] keyframes = _utilityNode.Function.keys;
//            keyframes[0].inWeight = _utilityNode.MinX;
//            keyframes[keyframes.Length - 1].inWeight = _utilityNode.MaxX;
//            _utilityNode.ScaledFunction.keys = keyframes;
        }

    }
}
