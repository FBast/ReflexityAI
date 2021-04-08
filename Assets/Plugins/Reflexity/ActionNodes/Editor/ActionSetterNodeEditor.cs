using System.Linq;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Plugins.Reflexity.ActionNodes.Editor {
    [CustomNodeEditor(typeof(ActionSetterNode))]
    public class ActionSetterNodeEditor : NodeEditor {

        private ActionSetterNode _actionSetterNode;

        public override void OnBodyGUI() {
            if (_actionSetterNode == null) _actionSetterNode = (ActionSetterNode) target;
            if (_actionSetterNode.SelectedSerializableInfo != null && _actionSetterNode.SerializableInfos.Count > 0) {
                NodePort valuePort = _actionSetterNode.GetPort(nameof(_actionSetterNode.Value));
                valuePort.ValueType = _actionSetterNode.SelectedSerializableInfo.Type;
                NodeEditorGUILayout.PortField(valuePort);
                string[] choices = _actionSetterNode.SerializableInfos.Select(info => info.Name).ToArray();
                //BUG-fred ArgumentException: Getting control 2's position in a group with only 2 controls when doing mouseUp
                int choiceIndex = EditorGUILayout.Popup(_actionSetterNode.ChoiceIndex, choices);
                if (choiceIndex != _actionSetterNode.ChoiceIndex) {
                    UpdateChoice(choiceIndex);
                }
                NodePort dataPort = _actionSetterNode.GetPort(nameof(_actionSetterNode.Data));
                NodeEditorGUILayout.AddPortField(dataPort);
                NodePort linkedOptionPort = _actionSetterNode.GetPort(nameof(_actionSetterNode.LinkedOption));
                NodeEditorGUILayout.AddPortField(linkedOptionPort);
            } else {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_actionSetterNode.Data)));
            }
        }

        public void UpdateChoice(int choiceIndex) {
            _actionSetterNode.ChoiceIndex = choiceIndex;
            _actionSetterNode.SelectedSerializableInfo = _actionSetterNode.SerializableInfos.ElementAt(_actionSetterNode.ChoiceIndex);
        }
        
    }
}