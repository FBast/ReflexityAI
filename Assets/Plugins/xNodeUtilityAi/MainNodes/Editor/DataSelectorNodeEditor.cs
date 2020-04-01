using System;
using System.Linq;
using Plugins.xNodeUtilityAi.Utils;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Plugins.xNodeUtilityAi.MainNodes.Editor {
    [CustomNodeEditor(typeof(DataSelectorNode))]
    public class DataSelectorNodeEditor : NodeEditor {

        private DataSelectorNode _dataSelectorNode;

        public override void OnBodyGUI() {
            if (_dataSelectorNode == null) _dataSelectorNode = (DataSelectorNode) target;
            serializedObject.Update();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_dataSelectorNode.Data)));
            if (_dataSelectorNode.MemberInfos.Count > 0) {
                string[] choices = _dataSelectorNode.MemberInfos.Select(info => info.Name).ToArray();
                _dataSelectorNode.ChoiceIndex = EditorGUILayout.Popup(_dataSelectorNode.ChoiceIndex, choices);
                _dataSelectorNode.SelectedMemberInfo = _dataSelectorNode.MemberInfos.ElementAt(_dataSelectorNode.ChoiceIndex);
                NodePort nodePort = _dataSelectorNode.GetPort(nameof(_dataSelectorNode.Output));
                nodePort.ValueType = _dataSelectorNode.SelectedMemberInfo.FieldType();
                NodeEditorGUILayout.AddPortField(nodePort);
            }
            serializedObject.ApplyModifiedProperties();
        }
        
        // public override void OnBodyGUI() {
        //     if (_dataSelectorNode == null) _dataSelectorNode = (DataSelectorNode) target;
        //     serializedObject.Update();
        //     NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_dataSelectorNode.Data)));
        //     if (_dataSelectorNode.MemberInfos.Count > 0) {
        //         string[] choices = _dataSelectorNode.MemberInfos.Select(info => info.Name).ToArray();
        //         _choiceIndex = EditorGUILayout.Popup(_choiceIndex, choices);
        //         _dataSelectorNode.SelectedMemberInfo = _dataSelectorNode.MemberInfos.ElementAt(_choiceIndex);
        //         NodePort nodePort = _dataSelectorNode.GetPort(nameof(_dataSelectorNode.Output));
        //         nodePort.ValueType = _dataSelectorNode.SelectedMemberInfo.FieldType();
        //         NodeEditorGUILayout.AddPortField(nodePort);
        //     }
        //     serializedObject.ApplyModifiedProperties();
        // }

    }
}