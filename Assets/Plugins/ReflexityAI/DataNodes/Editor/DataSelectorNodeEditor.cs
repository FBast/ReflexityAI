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
                int choiceIndex = EditorGUILayout.Popup(_dataSelectorNode.ChoiceIndex, choices);
                if (choiceIndex != _dataSelectorNode.ChoiceIndex) {
                    UpdateChoice(choiceIndex);
                }
                _dataSelectorNode.SelectedSerializableInfo = _dataSelectorNode.SerializableInfos.ElementAt(_dataSelectorNode.ChoiceIndex);
                NodePort dataPort = _dataSelectorNode.GetPort(nameof(_dataSelectorNode.Data));
                NodeEditorGUILayout.AddPortField(dataPort);
                NodePort outputPort = _dataSelectorNode.GetPort(nameof(_dataSelectorNode.Output));
                outputPort.ValueType = _dataSelectorNode.SelectedSerializableInfo.Type;
                NodeEditorGUILayout.AddPortField(outputPort);
            }
            else {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_dataSelectorNode.Data)));
            }
            serializedObject.ApplyModifiedProperties();
        }
        
        public void UpdateChoice(int choiceIndex) {
            _dataSelectorNode.ChoiceIndex = choiceIndex;
            foreach (NodePort nodePort in _dataSelectorNode.GetPort(nameof(_dataSelectorNode.Output)).GetConnections()) {
                nodePort.ClearConnections();
            }
        }
        
    }
}