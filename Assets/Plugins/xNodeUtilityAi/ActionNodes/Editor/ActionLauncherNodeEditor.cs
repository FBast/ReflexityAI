using System;
using System.Linq;
using Plugins.xNodeUtilityAi.Framework;
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
            if (_actionLauncherNode.SerializableMethodInfos.Count > 0) {
                foreach (NodePort dynamicInput in _actionLauncherNode.DynamicInputs) {
                    NodeEditorGUILayout.PortField(dynamicInput);
                }
                string[] choices = _actionLauncherNode.SerializableMethodInfos.Select(info => info.Name).ToArray();
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
            _actionLauncherNode.SelectedSerializableMethodInfo = _actionLauncherNode.SerializableMethodInfos
                .ElementAt(_actionLauncherNode.ChoiceIndex);
            _actionLauncherNode.ClearDynamicPorts();
            foreach (SerializableMethodInfo.Parameter parameter in _actionLauncherNode.SelectedSerializableMethodInfo.Parameters) {
                Type parameterType = Type.GetType(parameter.TypeName);
                _actionLauncherNode.AddDynamicInput(parameterType, Node.ConnectionType.Override, Node.TypeConstraint.InheritedInverse, parameter.Name);
            }
        }

    }
}