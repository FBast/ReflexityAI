using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilityAI.Nodes;
using Random = UnityEngine.Random;

namespace UtilityAI {
    public class AbstractAiComponent<T, U> : MonoBehaviour 
        where U : AbstractAIBrain<T> {
        
        [Serializable]
        public class DualUtility {

            public ActionNode<T> ActionNode;
            public string ActionName;
            public float Rank;
            public float Weight;

            public DualUtility(ActionNode<T> actionNode, float rank) {
                ActionNode = actionNode;
                ActionName = actionNode.name;
                Rank = rank;
            }
			
        }
        
        public U UtilityAiBrain;
        public float LastProbabilityResult;
        public List<DualUtility> DualUtilityReasoners;
        
        protected DualUtility ChooseAction(T context) {
            if (UtilityAiBrain == null) return null;
            UtilityAiBrain.GetEntryNode().ForEach(node => node.SetContext(context));
            // Fill the Dual Utilities
            DualUtilityReasoners = new List<DualUtility>();
            UtilityAiBrain.GetActionNode().ForEach(node => DualUtilityReasoners.Add(new DualUtility(node, node.GetValue())));
            // Remove ImpossibleDecisionValue Utilities
            DualUtilityReasoners.RemoveAll(reasoner => reasoner.Rank <= 0f);
            // If no more decision then return
            if (DualUtilityReasoners.Count == 0)
                return null;
            // Calculating Weights
            foreach (DualUtility dualreasoner in DualUtilityReasoners) {
                dualreasoner.Weight = dualreasoner.Rank / DualUtilityReasoners.Sum(reasoner => reasoner.Rank);
            }
            // Sorting by Utility values
            DualUtilityReasoners = DualUtilityReasoners.OrderByDescending(tuple => tuple.Rank).ToList();
            // Removing lesser value than maximum rank
            DualUtilityReasoners.RemoveAll(reasoner => reasoner.Rank < DualUtilityReasoners[0].Rank);
            // Rolling probability on weighted random
            LastProbabilityResult = Random.Range(0f, 1f);
            float weightSum = 0f;
            foreach (DualUtility dualReasoner in DualUtilityReasoners) {
                weightSum += dualReasoner.Weight;
                if (weightSum >= LastProbabilityResult)
                    return dualReasoner;
            }
            return null;
        }
        
    }
}