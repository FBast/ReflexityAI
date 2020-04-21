using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.MainNodes;

namespace Plugins.xNodeUtilityAi.Framework {
    [Serializable]
    public class AIOption {

        public OptionNode OptionNode;
        public List<AIAction> AiActions = new List<AIAction>();
        public float Rank;
        public int Weight;
        public float Probability;
        public string Description;

        public AIOption(OptionNode optionNode) {
            // Saving linked optionNode
            OptionNode = optionNode;
            // Processing actions
            List<ActionNode> actionNodes = OptionNode.GetInputPort(nameof(OptionNode.Actions)).GetInputValues<ActionNode>().ToList();
            actionNodes.ForEach(node => AiActions.Add(new AIAction(node)));
            AIAction actionWithData = AiActions.FirstOrDefault(action => action.Data != null);
            // Building description
            Description = OptionNode.Description;
            if (actionWithData == null) return;
            foreach (object data in actionWithData.Data) {
                Description += " " + data;
            }
        }

        public void UpdateWeight() {
            Weight = OptionNode.GetWeight();
        }

        public void UpdateRank() {
            Rank = OptionNode.GetRank();
        }

        public void ExecuteActions() {
            AiActions = AiActions.OrderBy(action => action.Order).ToList();
            AiActions.ForEach(action => action.Action.Invoke(action.Context, action.Data));
        }

        public override string ToString() {
            return Description + " - Rank " + Rank + " - Weight " + Weight + " - Probability " + Probability;
        }

    }
}