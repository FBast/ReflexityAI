using System;
using Plugins.xNodeUtilityAi.AbstractNodes;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.Framework {
    public class AIAction {

        public Action<AbstractAIComponent, Object> Action;
        public Object Data;
        public int Order;

        public AIAction(ActionNode actionNode) {
            Action = actionNode.Execute;
            Data = actionNode.GetData();
            Order = actionNode.Order;
        }

    }
}