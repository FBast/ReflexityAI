using XNode;
using XNodeEditor;

namespace Plugins.xNodeUtilityAi.MainNodes.Editor {
    [CustomNodeEditor(typeof(DataIteratorNode))]
    public class DataIteratorNodeEditor : NodeEditor {

        private DataIteratorNode _dataIteratorNode;

        // public override void OnBodyGUI() {
        //     if (_dataIteratorNode == null) _dataIteratorNode = (DataIteratorNode) target;
        //     serializedObject.Update();
        //     NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_dataIteratorNode.DataList)));
        //     if (_dataIteratorNode.IteratedReflectionData != null) {
        //         NodePort nodePort = _dataIteratorNode.GetPort(nameof(_dataIteratorNode.DataList));
        //         nodePort.ValueType = _dataIteratorNode.IteratedReflectionData.Type;
        //         NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_dataIteratorNode.Data)));
        //     }
        //     serializedObject.ApplyModifiedProperties();
        // }

    }
}