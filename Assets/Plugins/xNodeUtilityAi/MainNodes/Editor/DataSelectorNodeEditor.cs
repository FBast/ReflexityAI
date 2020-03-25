using System.Linq;
using System.Text.RegularExpressions;
using Plugins.xNodeUtilityAi.Utils;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Plugins.xNodeUtilityAi.MainNodes.Editor {
    [CustomNodeEditor(typeof(DataSelectorNode))]
    public class DataSelectorNodeEditor : NodeEditor {

        private DataSelectorNode _dataSelectorNode;
        private int _choiceIndex;
        private string _search = "";

        public override void OnBodyGUI() {
            if (_dataSelectorNode == null) _dataSelectorNode = (DataSelectorNode) target;
            serializedObject.Update();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_dataSelectorNode.Data)));
            if (_dataSelectorNode.MemberInfos.Count > 0) {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Search");
                _search = EditorGUILayout.TextField(_search);
                EditorGUILayout.EndHorizontal();
                string[] choices = _dataSelectorNode.MemberInfos.Select(info => info.Name).ToArray();
                if (!string.IsNullOrEmpty(_search)) {
                    Regex regex = new Regex(_search);
                    choices =  choices.Where(s => regex.IsMatch(s)).ToArray();
                }
                _choiceIndex = EditorGUILayout.Popup(_choiceIndex, choices);
                _dataSelectorNode.SelectedMemberInfo = _dataSelectorNode.MemberInfos.ElementAt(_choiceIndex);
                NodePort nodePort = _dataSelectorNode.GetPort(nameof(_dataSelectorNode.Output));
                nodePort.ValueType = _dataSelectorNode.SelectedMemberInfo.FieldType();
                NodeEditorGUILayout.AddPortField(nodePort);
            }
            serializedObject.ApplyModifiedProperties();
        }

    }

}