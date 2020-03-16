using System;
using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.MainNodes;
using XNode;
using XNodeEditor;

namespace Plugins.xNodeUtilityAi.Editor {
    [CustomNodeGraphEditor(typeof(AIBrain))]
    public class AIBrainGraphEditor : NodeGraphEditor {

        public override string GetNodeMenuName(Type type) {
            if (type == typeof(OptionNode)) {
                return "MainNodes/OptionNode";
            }
            if (type == typeof(UtilityNode)) {
                return "MainNodes/UtilityNode";
            }
            if (type.IsSubclassOf(typeof(MiddleNode))) {
                return "MiddleNodes/" + NodeEditorUtilities.NodeDefaultName(type);
            }
            if (type.IsSubclassOf(typeof(DataEntryNode))) {
                return "EntryNodes/DataEntryNodes/" + NodeEditorUtilities.NodeDefaultName(type);
            }
            if (type.IsSubclassOf(typeof(SimpleEntryNode))) {
                return "EntryNodes/SimpleEntryNodes/" + NodeEditorUtilities.NodeDefaultName(type);
            }
            if (type.IsSubclassOf(typeof(CollectionEntryNode))) {
                return "EntryNodes/CollectionEntryNodes/" + NodeEditorUtilities.NodeDefaultName(type);
            }
            if (type.IsSubclassOf(typeof(DataActionNode))) {
                return "ActionNodes/DataActionNodes/" + NodeEditorUtilities.NodeDefaultName(type);
            }
            if (type.IsSubclassOf(typeof(SimpleActionNode))) {
                return "ActionNodes/SimpleActionNodes/" + NodeEditorUtilities.NodeDefaultName(type);
            }
            return null;
        }

        public override string GetPortTooltip(NodePort port) {
            try {
                Type portType = port.ValueType;
                string tooltip = "";
                tooltip = portType.PrettyName();
                if (port.IsOutput) {
                    object obj = port.node.GetValue(port);
                    tooltip += " = " + (obj != null ? obj.ToString() : "null");
                }
                return tooltip;
            } catch (Exception exception) {
                return "Unable to recover port value";
            }
        }

    }
}
