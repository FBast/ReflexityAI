using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Plugins.xNodeUtilityAi.MainNodes.Editor {
    [CustomNodeEditor(typeof(DataSelectorNode))]
    public class DataSelectorNodeEditor : NodeEditor {

        private KeyValuePair<string, FieldInfo> _choice;
        private int _choiceIndex;
        
        private DataSelectorNode _dataSelectorNode;

        public override void OnBodyGUI() {
            if (_dataSelectorNode == null) 
                _dataSelectorNode = target as DataSelectorNode;
            if (_dataSelectorNode == null) return;
            // NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_dataSelectorNode.Data)));
            if (_dataSelectorNode.Fields.Count > 0) {
                IEnumerable<string> availablePorts = _dataSelectorNode.Fields.Select(pair => pair.Key)
                    .Except(_dataSelectorNode.SelectedFields);
                _choiceIndex = EditorGUILayout.Popup(_choiceIndex, availablePorts.ToArray());
                _choice = _dataSelectorNode.Fields.ElementAt(_choiceIndex);
                if (GUILayout.Button("Add")) {
                    _dataSelectorNode.AddDynamicOutput(_choice.Value.FieldType, fieldName: _choice.Key);
                    _dataSelectorNode.SelectedFields.Add(_choice.Key);
                    // EditorUtility.SetDirty(target);
                }
            }
            // NodeEditorGUILayout.DynamicPortList(nameof(_dataSelectorNode.SelectedFields), _dataSelectorNode.SelectedFields.GetType(), 
            //     serializedObject, NodePort.IO.Output, Node.ConnectionType.Override, Node.TypeConstraint.None, delegate(ReorderableList list) {
            //         list.displayAdd = false; 
            //     });
            base.OnBodyGUI();
        }

    }
}