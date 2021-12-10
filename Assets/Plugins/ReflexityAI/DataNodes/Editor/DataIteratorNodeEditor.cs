using XNode;
using XNodeEditor;

namespace Plugins.Reflexity.DataNodes.Editor {
    [CustomNodeEditor(typeof(DataIteratorNode))]
    public class DataIteratorNodeEditor : NodeEditor {

        private DataIteratorNode _dataIteratorNode;

        public override void OnBodyGUI() {
            if (_dataIteratorNode == null) _dataIteratorNode = (DataIteratorNode) target;
            serializedObject.Update();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_dataIteratorNode.Enumerable)));
            if (_dataIteratorNode.GetInputPort(nameof(_dataIteratorNode.Enumerable)).IsConnected && _dataIteratorNode.ArgumentType != null) {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_dataIteratorNode.LinkedOption)));
                NodePort nodePort = _dataIteratorNode.GetOutputPort(_dataIteratorNode.ArgumentType.Name);
                NodeEditorGUILayout.PortField(nodePort);
            }
            serializedObject.ApplyModifiedProperties();
        }    

    }
}