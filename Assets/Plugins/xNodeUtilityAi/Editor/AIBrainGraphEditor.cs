using System;
using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.AbstractNodes.DataNodes;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.MainNodes;
using Plugins.xNodeUtilityAi.MiddleNodes;
using XNode;
using XNodeEditor;

namespace Plugins.xNodeUtilityAi.Editor {
    [CustomNodeGraphEditor(typeof(AIBrainGraph))]
    public class AIBrainGraphEditor : NodeGraphEditor {

        public override string GetNodeMenuName(Type type) {
            if (type == typeof(OptionNode)) {
                return "MainNodes/" + NodeEditorUtilities.NodeDefaultName(type);
            }
            if (type == typeof(UtilityNode)) {
                return "MainNodes/" + NodeEditorUtilities.NodeDefaultName(type);
            }
            if (type == typeof(ConverterNode)) {
                return "MainNodes/"  + NodeEditorUtilities.NodeDefaultName(type);
            }
            if (type.IsSubclassOf(typeof(EntryNode))) {
                return "EntryNodes/"  + NodeEditorUtilities.NodeDefaultName(type);
            }
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
                if (port.IsOutput) {
                    object obj = port.node.GetValue(port);
                    tooltip += " = " + (obj != null ? obj.ToString() : "null");
                }
                return tooltip;
            } catch (Exception) {
                return "Unable to recover port value";
            }
        }

    }
}
