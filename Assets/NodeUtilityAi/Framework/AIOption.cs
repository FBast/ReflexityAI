using System;
using System.Collections.Generic;
using System.Linq;
using NodeUtilityAi.Nodes;

namespace NodeUtilityAi.Framework {
    [Serializable]
    public class AIOption {

        public List<AIAction> AiActions = new List<AIAction>();
        public float Utility;
        public int Rank;
        public float Weight;
        public string Description;

        public AIOption(List<ActionNode> actionNodes, Tuple<float, int> utility, string description) {
            // Processing Simple Actions
            actionNodes.ForEach(node => AiActions.Add(new AIAction(node.Execute, node.GetData(), node.Order)));
            // Processing Utility
            Utility = utility.Item1;
            Rank = utility.Item2;
            AIAction aiActionWithMoreData = AiActions.OrderByDescending(action => action.AiData.Count).First();
            Description = description + string.Join(" ", aiActionWithMoreData.AiData.ToString());
        }

        public void ExecuteActions(AbstractAIComponent context) {
            AiActions = AiActions.OrderBy(action => action.Order).ToList();
            AiActions.ForEach(action => action.Action.Invoke(context, action.AiData));
        }
        
    }

}