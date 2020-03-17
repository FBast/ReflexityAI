using System;
using System.Collections;
using System.Collections.Generic;
using Plugins.xNodeUtilityAi.AbstractNodes;

namespace Plugins.xNodeUtilityAi.Framework {
    public class AIAction {

        public Action<AbstractAIComponent, AIData> Action;
        public AIData AiData = new AIData();
        public int Order;

        public AIAction(ActionNode actionNode) {
            Action = actionNode.Execute;
            // Remove empty data
            actionNode.GetData().RemoveAll(data => data == null);
            actionNode.GetData().ForEach(data => AiData.Add(data.DataTag, data.Data));
            Order = actionNode.Order;
        }
        
        public AIAction(Action<AbstractAIComponent, AIData> action, List<TaggedData> taggedDatas, int order) {
            Action = action;
            if (taggedDatas != null) {
                // Remove empty data
                taggedDatas.RemoveAll(data => data == null);
                taggedDatas.ForEach(data => AiData.Add(data.DataTag, data.Data));
            }
            Order = order;
        }
        
    }
}