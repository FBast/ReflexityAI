using XNode;
using XNodeEditor;

namespace Plugins.ReflexityAI.DataNodes.Editor {
    [CustomNodeEditor(typeof(DataIteratorNode))]
    public class DataIteratorNodeEditor : NodeEditor {

        private DataIteratorNode _dataIteratorNode;

        public override void OnBodyGUI() {
            if (_dataIteratorNode == null) _dataIteratorNode = (DataIteratorNode) target;
            serializedObject.Update();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_dataIteratorNode.Enumerable)));
            if (_dataIteratorNode.GetInputPort(nameof(_dataIteratorNode.Enumerable)).IsConnected) {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_dataIteratorNode.LinkedOption)));
                NodeEditorGUILayout.PortField(_dataIteratorNode.GetOutputPort(_dataIteratorNode.ArgumentType.Name));
//                NodePort nodePort = _dataIteratorNode.GetPort(nameof(_dataIteratorNode.Output));
//                NodeEditorGUILayout.PortField(nodePort);
            }
            serializedObject.ApplyModifiedProperties();
        }    

    }
}