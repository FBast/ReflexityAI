using System.Linq;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Plugins.ReflexityAI.ActionNodes.Editor {
    [CustomNodeEditor(typeof(ActionSetterNode))]
    public class ActionSetterNodeEditor : NodeEditor {

        private ActionSetterNode _actionSetterNode;

        public override void OnBodyGUI() {
            if (_actionSetterNode == null) _actionSetterNode = (ActionSetterNode) target;
            serializedObject.Update();
            if (_actionSetterNode.SelectedSerializableInfo != null && _actionSetterNode.SerializableInfos.Count > 0) {
                NodePort valuePort = _actionSetterNode.GetPort(nameof(_actionSetterNode.Value));
                valuePort.ValueType = _actionSetterNode.SelectedSerializableInfo.Type;
                NodeEditorGUILayout.PortField(valuePort);
                string[] choices = _actionSetterNode.SerializableInfos.Select(info => info.Name).ToArray();
                int choiceIndex = EditorGUILayout.Popup(_actionSetterNode.ChoiceIndex, choices);
                if (choiceIndex != _actionSetterNode.ChoiceIndex) {
                    UpdateChoice(choiceIndex);
                }
                _actionSetterNode.SelectedSerializableInfo = _actionSetterNode.SerializableInfos.ElementAt(_actionSetterNode.ChoiceIndex);
                NodePort dataPort = _actionSetterNode.GetPort(nameof(_actionSetterNode.Data));
                NodeEditorGUILayout.AddPortField(dataPort);
                NodePort linkedOptionPort = _actionSetterNode.GetPort(nameof(_actionSetterNode.LinkedOption));
                NodeEditorGUILayout.AddPortField(linkedOptionPort);
            } 
            else {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_actionSetterNode.Data)));
            }
            serializedObject.ApplyModifiedProperties();
        }

        public void UpdateChoice(int choiceIndex) {
            _actionSetterNode.ChoiceIndex = choiceIndex;
            foreach (NodePort nodePort in _actionSetterNode.GetPort(nameof(_actionSetterNode.Value)).GetConnections()) {
                nodePort.ClearConnections();
            }
        }
        
    }
}