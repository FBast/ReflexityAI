using System.Linq;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Plugins.ReflexityAI.DataNodes.Editor {
    [CustomNodeEditor(typeof(DataSelectorNode))]
    public class DataSelectorNodeEditor : NodeEditor {

        private DataSelectorNode _dataSelectorNode;

        public override void OnBodyGUI() {
            if (_dataSelectorNode == null) _dataSelectorNode = (DataSelectorNode) target;
            serializedObject.Update();
            if (_dataSelectorNode.SerializableInfos.Count > 0) {
                string[] choices = _dataSelectorNode.SerializableInfos.Select(info => info.Name).ToArray();
                _dataSelectorNode.ChoiceIndex = EditorGUILayout.Popup(_dataSelectorNode.ChoiceIndex, choices);
                _dataSelectorNode.SelectedSerializableInfo = _dataSelectorNode.SerializableInfos.ElementAt(_dataSelectorNode.ChoiceIndex);
                NodePort dataPort = _dataSelectorNode.GetPort(nameof(_dataSelectorNode.Data));
                NodeEditorGUILayout.AddPortField(dataPort);
                NodePort nodePort = _dataSelectorNode.GetPort(nameof(_dataSelectorNode.Output));
                nodePort.ValueType = _dataSelectorNode.SelectedSerializableInfo.Type;
                NodeEditorGUILayout.AddPortField(nodePort);
            }
            else {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_dataSelectorNode.Data)));
            }
            serializedObject.ApplyModifiedProperties();
        }
        
    }
}