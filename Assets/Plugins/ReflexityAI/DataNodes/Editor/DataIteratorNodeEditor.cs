using XNodeEditor;

namespace Plugins.ReflexityAI.DataNodes.Editor {
    [CustomNodeEditor(typeof(DataIteratorNode))]
    public class DataIteratorNodeEditor : NodeEditor {

        private DataIteratorNode _dataIteratorNode;

        // public override void OnBodyGUI() {
        //     if (_dataIteratorNode == null) _dataIteratorNode = (DataIteratorNode) target;
        //     serializedObject.Update();
        //     NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_dataIteratorNode.DataList)));
        //     bool isLinkedOptionConnected = _dataIteratorNode.GetOutputPort(nameof(_dataIteratorNode.LinkedOption)).IsConnected;
        //     Tuple<string,Type,object> reflectionData = _dataIteratorNode.IteratedReflectionData;
        //     if (reflectionData != null && isLinkedOptionConnected) {
        //         _dataIteratorNode.AddDynamicOutput(reflectionData.Item2, Node.ConnectionType.Multiple, 
        //             Node.TypeConstraint.Inherited, reflectionData.Item1);
        //     }
        //     serializedObject.ApplyModifiedProperties();
        // }    

    }
}