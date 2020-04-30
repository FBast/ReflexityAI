using System;
using Plugins.ReflexityAI.MainNodes;
using Plugins.ReflexityAI.MemoryNodes;
using Plugins.ReflexityAI.MiddleNodes;
using Plugins.ReflexityAI.PatternNodes;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Plugins.ReflexityAI.Framework.Editor {
    [CustomNodeGraphEditor(typeof(AIBrainGraph))]
    public class AIBrainGraphEditor : NodeGraphEditor {

        public override int GetNodeMenuOrder(Type type) {
            // Main Nodes
            if (type == typeof(OptionNode) || type == typeof(UtilityNode) || type == typeof(ConverterNode)) {
                return 1;
            }
            // Memory Nodes
            if (type == typeof(MemoryCheckNode) || type == typeof(MemoryClearNode) || type == typeof(MemoryLoadNode) || 
                type == typeof(MemorySaveNode)) {
                return 5;
            }
            // Pattern Nodes
            if (type == typeof(InCooldownNode) || type == typeof(SaveHistoricNode)) {
                return 6;
            }
            // Other Nodes
            if (type.IsSubclassOf(typeof(MiddleNode))) {
                return 4;
            }
            if (type.IsSubclassOf(typeof(DataNode))) {
                return 2;
            }
            if (type.IsSubclassOf(typeof(ActionNode))) {
                return 3;
            }
            return 0;
        }

        public override string GetNodeMenuName(Type type) {
            // Main Nodes
            if (type == typeof(OptionNode) || type == typeof(UtilityNode) || type == typeof(ConverterNode)) {
                return "MainNodes/" + NodeEditorUtilities.NodeDefaultName(type);
            }
            // Memory Nodes
            if (type == typeof(MemoryCheckNode) || type == typeof(MemoryClearNode) || type == typeof(MemoryLoadNode) || 
                type == typeof(MemorySaveNode)) {
                return null;
                //TODO-fred Memory Nodes not available yet
//                return "MemoryNodes/"  + NodeEditorUtilities.NodeDefaultName(type);
            }
            // Pattern Nodes
            if (type == typeof(InCooldownNode) || type == typeof(SaveHistoricNode)) {
                return null;
                //TODO-fred Pattern Nodes not available yet
//                return "PatternNodes/"  + NodeEditorUtilities.NodeDefaultName(type);
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
                string tooltip = portType.PrettyName();
                if (!Application.isPlaying || port.IsInput) return tooltip;
                object obj = port.node.GetValue(port);
                if (obj is ReflectionData reflectionData)
                    tooltip += " = " + (reflectionData.Value ?? "null");
                else
                    tooltip += " = " + (obj != null ? obj.ToString() : "null");
                return tooltip;
            } catch (Exception) {
                return "Unable to recover port value";
            }
        }

    }
}
