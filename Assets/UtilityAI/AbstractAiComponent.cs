using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilityAI.Nodes;
using Random = UnityEngine.Random;

namespace UtilityAI {
    public class AbstractAiComponent<T, U> : MonoBehaviour 
        where U : UtilityAIBrain<T> {
        
        [Serializable]
        public class UtilityReasoner {

            public ActionNode<T> ActionNode;
            public string ActionName;
            public float Utility;
            public float Weight;

            public UtilityReasoner(ActionNode<T> actionNode, float utility) {
                ActionNode = actionNode;
                ActionName = actionNode.name;
                Utility = utility;
            }
			
        }
        
        public U UtilityAiBrain;
        public float LastProbabilityResult;
        public List<UtilityReasoner> UtilityReasoners;
        
        protected UtilityReasoner ChooseAction(T context) {
            if (UtilityAiBrain == null) return null;
            UtilityAiBrain.GetEntryNode().ForEach(node => node.SetContext(context));
            // Fill the UtilityY Reasoner
            UtilityReasoners = new List<UtilityReasoner>();
            UtilityAiBrain.GetActionNode().ForEach(node => UtilityReasoners.Add(new UtilityReasoner(node, node.GetValue())));
            // Remove ImpossibleDecisionValue Utilities
            UtilityReasoners.RemoveAll(reasoner => reasoner.Utility <= 0f);
            // If no more decision then return
            if (UtilityReasoners.Count == 0)
                return null;
            // Calculating Weights
            foreach (UtilityReasoner utilityReasoner in UtilityReasoners) {
                utilityReasoner.Weight = utilityReasoner.Utility / UtilityReasoners.Sum(reasoner => reasoner.Utility);
            }
            // Sorting by Weights values
            UtilityReasoners = UtilityReasoners.OrderByDescending(tuple => tuple.Weight).ToList();
            LastProbabilityResult = Random.Range(0f, 1f);
            float weightSum = 0f;
            foreach (UtilityReasoner utilityReasoner in UtilityReasoners) {
                weightSum += utilityReasoner.Weight;
                if (weightSum >= LastProbabilityResult)
                    return utilityReasoner;
            }
            return null;
        }
        
    }
}