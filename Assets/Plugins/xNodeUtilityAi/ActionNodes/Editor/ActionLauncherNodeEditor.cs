using System;
using System.Linq;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Plugins.xNodeUtilityAi.ActionNodes.Editor {
    [CustomNodeEditor(typeof(ActionLauncherNode))]
    public class ActionLauncherNodeEditor : NodeEditor {

        private ActionLauncherNode _actionLauncherNode;

        public override void OnBodyGUI() {
            if (_actionLauncherNode == null) _actionLauncherNode = (ActionLauncherNode) target;
            serializedObject.Update();
            if (_actionLauncherNode.SerializableMemberInfos.Count > 0) {
                foreach (NodePort dynamicInput in _actionLauncherNode.DynamicInputs) {
                    NodeEditorGUILayout.PortField(dynamicInput);
                }
                string[] choices = _actionLauncherNode.SerializableMemberInfos.Select(info => info.Name).ToArray();
                //BUG-fred ArgumentException: Getting control 2's position in a group with only 2 controls when doing mouseUp
                int choiceIndex = EditorGUILayout.Popup(_actionLauncherNode.ChoiceIndex, choices);
                if (choiceIndex != _actionLauncherNode.ChoiceIndex) {
                    UpdateChoice(choiceIndex);
                }
                NodePort dataPort = _actionLauncherNode.GetPort(nameof(_actionLauncherNode.Data));
                NodeEditorGUILayout.AddPortField(dataPort);
                NodePort nodePort = _actionLauncherNode.GetPort(nameof(_actionLauncherNode.LinkedOption));
                NodeEditorGUILayout.AddPortField(nodePort);
            } else {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_actionLauncherNode.Data)));
            }
            serializedObject.ApplyModifiedProperties();
        }

        public void UpdateChoice(int choiceIndex) {
            _actionLauncherNode.ChoiceIndex = choiceIndex;
            _actionLauncherNode.SelectedSerializableMemberInfo = _actionLauncherNode.SerializableMemberInfos
                .ElementAt(_actionLauncherNode.ChoiceIndex);
            _actionLauncherNode.ClearDynamicPorts();
            foreach ((string name, string type) valueTuple in _actionLauncherNode.SelectedSerializableMemberInfo.Parameters) {
                Type parameterType = Type.GetType(valueTuple.type);
                _actionLauncherNode.AddDynamicInput(parameterType, Node.ConnectionType.Override, Node.TypeConstraint.InheritedInverse, valueTuple.name);
            }
        }

    }
}