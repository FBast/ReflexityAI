using System;

namespace Plugins.ReflexityAI.Framework {
    public class AIAction {

        public Action<object, object[]> Action;
        public object Context;
        public object[] Data;
        public int Order;

        public AIAction(ActionNode actionNode) {
            Action = actionNode.Execute;
            Context = actionNode.GetContext();
            Data = actionNode.GetParameters();
            Order = actionNode.Order;
        }

    }
}