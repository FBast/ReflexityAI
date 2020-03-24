using XNodeEditor;

namespace Plugins.xNodeUtilityAi.MainNodes.Editor {
    [CustomNodeEditor(typeof(DataReaderNode))]
    public class DataReaderNodeEditor : NodeEditor {
    
        private DataReaderNode _dataReaderNode;

        public override void OnCreate() {
            if (_dataReaderNode == null) 
                _dataReaderNode = target as DataReaderNode;
        }

        // public override void OnBodyGUI() {
        //     if (_dataReaderNode.graph is AIBrainGraph brainGraph && brainGraph.ContextType == null) {
        //         EditorGUILayout.LabelField("You should select a brain graph context type to read data");
        //     }
        // }
    
    }
}