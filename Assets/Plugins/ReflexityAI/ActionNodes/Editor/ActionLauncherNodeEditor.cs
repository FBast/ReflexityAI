using System;
using System.Linq;
using Plugins.ReflexityAI.Framework;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Plugins.ReflexityAI.ActionNodes.Editor {
    [CustomNodeEditor(typeof(ActionLauncherNode))]
    public class ActionLauncherNodeEditor : NodeEditor {

        private ActionLauncherNode _actionLauncherNode;

        public override void OnBodyGUI() {
            if (_actionLauncherNode == null) _actionLauncherNode = (ActionLauncherNode) target;
            serializedObject.Update();
            if (_actionLauncherNode.SerializableInfos.Count > 0) {
                foreach (NodePort dynamicInput in _actionLauncherNode.DynamicInputs) {
                    NodeEditorGUILayout.PortField(dynamicInput);
                }
                string[] choices = _actionLauncherNode.SerializableInfos.Select(info => info.Name).ToArray();
                //BUG-fred ArgumentException: Getting control 2's position in a group with only 2 controls when doing mouseUp
                int choiceIndex = EditorGUILayout.Popup(_actionLauncherNode.ChoiceIndex, choices);
                if (choiceIndex != _actionLauncherNode.ChoiceIndex) {
                    UpdateChoice(choiceIndex);
                }
                NodePort dataPort = _actionLauncherNode.GetPort(nameof(_actionLauncherNode.Data));
                NodeEditorGUILayout.AddPortField(dataPort);
                NodePort linkedOptionPort = _actionLauncherNode.GetPort(nameof(_actionLauncherNode.LinkedOption));
                NodeEditorGUILayout.AddPortField(linkedOptionPort);
            } else {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_actionLauncherNode.Data)));
            }
            serializedObject.ApplyModifiedProperties();
        }
        
        public void UpdateChoice(int choiceIndex) {
            _actionLauncherNode.ChoiceIndex = choiceIndex;
            _actionLauncherNode.SelectedSerializableInfo = _actionLauncherNode.SerializableInfos
                .ElementAt(_actionLauncherNode.ChoiceIndex);
            _actionLauncherNode.ClearDynamicPorts();
            foreach (Parameter parameter in _actionLauncherNode.SelectedSerializableInfo.Parameters) {
                Type parameterType = Type.GetType(parameter.TypeName);
                _actionLauncherNode.AddDynamicInput(parameterType, Node.ConnectionType.Override, Node.TypeConstraint.InheritedInverse, parameter.Name);
            }
        }

    }
}