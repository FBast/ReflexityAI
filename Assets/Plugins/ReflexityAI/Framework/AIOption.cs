using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.Reflexity.DataNodes;
using Plugins.Reflexity.MainNodes;
using Object = UnityEngine.Object;

namespace Plugins.Reflexity.Framework {
    [Serializable]
    public class AIOption {

        public OptionNode OptionNode;
        public List<AIAction> AiActions = new List<AIAction>();
        public int Rank;
        public int Weight;
        public float OverallRank;
        public float Probability;
        public string Description;

        public AIOption(OptionNode optionNode, DataIteratorNode dataIteratorNode = null) {
            // Saving linked optionNode
            OptionNode = optionNode;
            Description = OptionNode.Description;
            // Calculate weight
            Weight = OptionNode.GetWeight();
            // Calculate rank
            Rank = OptionNode.GetRank();
            // Fetch actions
            foreach (ActionNode actionNode in OptionNode.GetActions()) {
                AiActions.Add(new AIAction(actionNode));
            }
        }

        public void ExecuteActions() {
            AiActions = AiActions.OrderBy(action => action.Order).ToList();
            AiActions.ForEach(action => action.Action.Invoke(action.Context, action.Data));
        }

        public override string ToString() {
            AIAction actionWithData = AiActions.FirstOrDefault(action => action.Data != null);
            string description = Description;
            // Add description based on data (for iteration)
            if (actionWithData != null) {
                foreach (object data in actionWithData.Data) {
                    if (data is Object obj) 
                        description += " (" + obj.name + ")";
                    else 
                        description += " (" + data + ")";
                    
                }
            }
            return description + " - Weight (" + Weight + ") | Rank (" + Rank + ") | " +
                   "OverallRank (" + OverallRank + ") | Probability(" + Probability + ")";
        }

    }
}