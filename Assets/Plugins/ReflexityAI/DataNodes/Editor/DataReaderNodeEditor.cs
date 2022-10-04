using System.Collections.Generic;
using System.Linq;
using Plugins.ReflexityAI.Framework;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Plugins.ReflexityAI.DataNodes.Editor {
    [CustomNodeEditor(typeof(DataReaderNode))]
    public class DataReaderNodeEditor : NodeEditor {
    
        private DataReaderNode _dataReaderNode;
    
        public override void OnBodyGUI() {
            if (_dataReaderNode == null) _dataReaderNode = (DataReaderNode) target;
            serializedObject.Update();
            EditorGUILayout.LabelField("Iterated Data (Inherit from IEnumerable)", EditorStyles.boldLabel);
            foreach (KeyValuePair<string,SerializableInfo> valuePair in _dataReaderNode.InfoDictionary.OrderBy(pair => pair.Value.Order)) {
                if (!valuePair.Value.IsIteratable) continue;
                NodePort dataPort = _dataReaderNode.GetOutputPort(valuePair.Value.PortName);
                NodeEditorGUILayout.PortField(dataPort);
            }
            EditorGUILayout.LabelField("Simple Data", EditorStyles.boldLabel);
            foreach (KeyValuePair<string,SerializableInfo> valuePair in _dataReaderNode.InfoDictionary.OrderBy(pair => pair.Value.Order)) {
                if (valuePair.Value.IsIteratable) continue;
                NodePort dataPort = _dataReaderNode.GetOutputPort(valuePair.Value.PortName);
                NodeEditorGUILayout.PortField(dataPort);
            }
            serializedObject.ApplyModifiedProperties();
        }
    
    }
}