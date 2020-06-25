using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Plugins.ReflexityAI.MainNodes;

namespace Plugins.ReflexityAI.Framework {
    [Serializable]
    public class AIOption {

        public OptionNode OptionNode;
        public List<AIAction> AiActions = new List<AIAction>();
        public int Rank;
        public int Weight;
        public float OverallRank;
        public float Probability;
        public string Description;
        public int IteratorIndex;
        public bool IsRanked;

        public AIOption(OptionNode optionNode) {
            // Saving linked optionNode
            OptionNode = optionNode;
            Description = OptionNode.Description;
            // Calculate weight
            Weight = OptionNode.GetWeight();
            // Fetch actions
            foreach (ActionNode actionNode in OptionNode.GetActions()) {
                AiActions.Add(new AIAction(actionNode));
            }
            // Saving iterator current output
            if (OptionNode.DataIteratorNode != null) {
                IteratorIndex = OptionNode.DataIteratorNode.Index;
            }
        }

        public void CalculateRank() {
            if (OptionNode.DataIteratorNode != null) {
                OptionNode.DataIteratorNode.Index = IteratorIndex;
            }
            Rank = OptionNode.GetRank();
            IsRanked = true;
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
                    description += " " + data;
                }
            }
            if (IsRanked) 
                return description + " - Weight(" + Weight + ") | Rank (" + Rank + ") | " +
                       "OverallRank (" + OverallRank + ") | Probability(" + Probability + ")";
            return description + " - Weight (" + Weight + ") = Probability(" + Probability + ")";
        }

    }
}