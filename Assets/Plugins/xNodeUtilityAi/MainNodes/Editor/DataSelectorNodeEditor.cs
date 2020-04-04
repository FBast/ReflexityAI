using System.Linq;
using System.Reflection;
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
            if (_dataSelectorNode.SerializableMemberInfos.Count > 0) {
                string[] choices = _dataSelectorNode.SerializableMemberInfos.Select(info => info.FieldName).ToArray();
                _dataSelectorNode.ChoiceIndex = EditorGUILayout.Popup(_dataSelectorNode.ChoiceIndex, choices);
                _dataSelectorNode.SelectedSerializableMemberInfo = _dataSelectorNode.SerializableMemberInfos
                    .ElementAt(_dataSelectorNode.ChoiceIndex);
                MemberInfo memberInfo = _dataSelectorNode.SelectedSerializableMemberInfo.ToMemberInfo();
                NodePort nodePort = _dataSelectorNode.GetPort(nameof(_dataSelectorNode.Output));
                nodePort.ValueType = memberInfo.FieldType();
                NodeEditorGUILayout.AddPortField(nodePort);
            }
            serializedObject.ApplyModifiedProperties();
        }

    }
}