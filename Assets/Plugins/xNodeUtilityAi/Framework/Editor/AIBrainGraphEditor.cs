using System;
using Plugins.xNodeUtilityAi.MainNodes;
using Plugins.xNodeUtilityAi.MemoryNodes;
using Plugins.xNodeUtilityAi.MiddleNodes;
using Plugins.xNodeUtilityAi.PatternNodes;
using XNode;
using XNodeEditor;

namespace Plugins.xNodeUtilityAi.Framework.Editor {
    [CustomNodeGraphEditor(typeof(AIBrainGraph))]
    public class AIBrainGraphEditor : NodeGraphEditor {

        public override string GetNodeMenuName(Type type) {
            // Main Nodes
            if (type == typeof(OptionNode)) {
                return "MainNodes/" + NodeEditorUtilities.NodeDefaultName(type);
            }
            if (type == typeof(UtilityNode)) {
                return "MainNodes/" + NodeEditorUtilities.NodeDefaultName(type);
            }
            if (type == typeof(ConverterNode)) {
                return "MainNodes/"  + NodeEditorUtilities.NodeDefaultName(type);
            }
            // Memory Nodes
            if (type == typeof(MemoryCheckNode)) {
                return "MemoryNodes/"  + NodeEditorUtilities.NodeDefaultName(type);
            }
            if (type == typeof(MemoryClearNode)) {
                return "MemoryNodes/"  + NodeEditorUtilities.NodeDefaultName(type);
            }
            if (type == typeof(MemoryLoadNode)) {
                return "MemoryNodes/"  + NodeEditorUtilities.NodeDefaultName(type);
            }
            if (type == typeof(MemorySaveNode)) {
                return "MemoryNodes/"  + NodeEditorUtilities.NodeDefaultName(type);
            }
            // Pattern Nodes
            if (type == typeof(InCooldownNode)) {
                return "PatternNodes/"  + NodeEditorUtilities.NodeDefaultName(type);
            }
            if (type == typeof(SaveHistoricNode)) {
                return "PatternNodes/"  + NodeEditorUtilities.NodeDefaultName(type);
            }
            // Other Nodes
            if (type.IsSubclassOf(typeof(MiddleNode))) {
                return "MiddleNodes/" + NodeEditorUtilities.NodeDefaultName(type);
            }
            if (type.IsSubclassOf(typeof(DataNode))) {
                return "DataNodes/" + NodeEditorUtilities.NodeDefaultName(type);
            }
            if (type.IsSubclassOf(typeof(ActionNode))) {
                return "ActionNodes/" + NodeEditorUtilities.NodeDefaultName(type);
            }
            return null;
        }

        public override string GetPortTooltip(NodePort port) {
            try {
                Type portType = port.ValueType;
                return portType.PrettyName();
            } catch (Exception) {
                return "Unable to recover port value";
            }
        }

    }
}
