using System.Linq;
using Plugins.xNodeUtilityAi.Framework;
using UnityEditor;
using XNodeEditor;

namespace Plugins.xNodeUtilityAi.DataNodes.Editor {
    [CustomNodeEditor(typeof(DataReaderNode))]
    public class DataReaderNodeEditor : NodeEditor {
    
        private DataReaderNode _dataReaderNode;
    
        public override void OnBodyGUI() {
            if (_dataReaderNode == null) _dataReaderNode = (DataReaderNode) target;
            serializedObject.Update();
            EditorGUILayout.LabelField("Iterated Data", EditorStyles.boldLabel);
            foreach (SerializableFieldOrProperty serializableData in _dataReaderNode.SerializableDatas
                .Where(info => info.IsIteratable).OrderBy(info => info.Order)) {
                NodeEditorGUILayout.PortField(_dataReaderNode.GetOutputPort(serializableData.PortName));
            }
            EditorGUILayout.LabelField("Simple Data", EditorStyles.boldLabel);
            foreach (SerializableFieldOrProperty serializableData in _dataReaderNode.SerializableDatas
                .Where(info => !info.IsIteratable).OrderBy(info => info.Order)) {
                NodeEditorGUILayout.PortField(_dataReaderNode.GetOutputPort(serializableData.PortName));
            }
            serializedObject.ApplyModifiedProperties();
        }
    
    }
}