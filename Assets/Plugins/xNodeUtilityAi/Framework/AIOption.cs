using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.MainNodes;

namespace Plugins.xNodeUtilityAi.Framework {
    [Serializable]
    public class AIOption {

        public List<AIAction> AiActions = new List<AIAction>();
        public float Rank;
        public int Weight;
        public float Probability;
        public string Description;
//        public int IteratorIndex;

        public AIOption(OptionNode optionNode) {
            // Saving linked optionNode
            Description = optionNode.Description;
            Weight = optionNode.GetWeight();
            Rank = optionNode.GetRank();
            List<ActionNode> actionNodes = optionNode.GetInputPort(nameof(optionNode.Actions)).GetInputValues<ActionNode>().ToList();
            foreach (ActionNode actionNode in actionNodes) {
                AiActions.Add(new AIAction(actionNode));
            }
//            // Saving iterator current output
//            if (optionNode.DataIteratorNode != null) {
//                IteratorIndex = optionNode.DataIteratorNode.Index;
//            }
        }

//        public void CalculateWeight() {
//            if (OptionNode.DataIteratorNode != null) {
//                OptionNode.DataIteratorNode.Index = IteratorIndex;
//            }
//            Weight = OptionNode.GetWeight();
//        }
//
//        public void CalculateRank() {
//            if (OptionNode.DataIteratorNode != null) {
//                OptionNode.DataIteratorNode.Index = IteratorIndex;
//            }
//            Rank = OptionNode.GetRank();
//        }
//
//        public void UpdateActions() {
//            if (OptionNode.DataIteratorNode != null) {
//                OptionNode.DataIteratorNode.Index = IteratorIndex;
//            }
//            List<ActionNode> actionNodes = OptionNode.GetInputPort(nameof(OptionNode.Actions)).GetInputValues<ActionNode>().ToList();
//            actionNodes.ForEach(node => AiActions.Add(new AIAction(node)));
//        }

        public void ExecuteActions() {
            AiActions = AiActions.OrderBy(action => action.Order).ToList();
            AiActions.ForEach(action => action.Action.Invoke(action.Context, action.Data));
        }

        public override string ToString() {
            AIAction actionWithData = AiActions.FirstOrDefault(action => action.Data != null);
            string description = Description;
            if (actionWithData != null) {
                foreach (object data in actionWithData.Data) {
                    description += " " + data;
                }
            }
            return description + " - Rank " + Rank + " - Weight " + Weight + " - Probability " + Probability;
        }

    }
}