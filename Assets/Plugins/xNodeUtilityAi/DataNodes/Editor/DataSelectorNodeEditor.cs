using System.Linq;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Plugins.xNodeUtilityAi.DataNodes.Editor {
    [CustomNodeEditor(typeof(DataSelectorNode))]
    public class DataSelectorNodeEditor : NodeEditor {

        private DataSelectorNode _dataSelectorNode;

        public override void OnBodyGUI() {
            if (_dataSelectorNode == null) _dataSelectorNode = (DataSelectorNode) target;
            serializedObject.Update();
            if (_dataSelectorNode.SerializableDatas.Count > 0) {
                string[] choices = _dataSelectorNode.SerializableDatas.Select(info => info.Name).ToArray();
                //BUG-fred ArgumentException: Getting control 2's position in a group with only 2 controls when doing mouseUp
                _dataSelectorNode.ChoiceIndex = EditorGUILayout.Popup(_dataSelectorNode.ChoiceIndex, choices);
                _dataSelectorNode.selectedSerializableFieldOrProperty = _dataSelectorNode.SerializableDatas
                    .ElementAt(_dataSelectorNode.ChoiceIndex);
                NodePort dataPort = _dataSelectorNode.GetPort(nameof(_dataSelectorNode.Data));
                NodeEditorGUILayout.AddPortField(dataPort);
                NodePort nodePort = _dataSelectorNode.GetPort(nameof(_dataSelectorNode.Output));
                nodePort.ValueType = _dataSelectorNode.selectedSerializableFieldOrProperty.Type;
                NodeEditorGUILayout.AddPortField(nodePort);
            } else {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_dataSelectorNode.Data)));
            }
            serializedObject.ApplyModifiedProperties();
        }
        
    }
}