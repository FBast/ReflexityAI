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

            public OptionNode<T> OptionNode;
            public string ActionName;
            public float Utility;
            public int Rank;
            public float Weight;

            public DualUtility(OptionNode<T> optionNode, Tuple<float, int> utilityAndRank) {
                OptionNode = optionNode;
                ActionName = optionNode.name;
                Utility = utilityAndRank.Item1;
                Rank = utilityAndRank.Item2;
            }
			
        }
        
        public U UtilityAiBrain;
        public float LastProbabilityResult;
        public List<DualUtility> DualUtilityReasoners;
        
        protected DualUtility ChooseAction(T context) {
            if (UtilityAiBrain == null) return null;
            UtilityAiBrain.GetEntryNodes().ForEach(node => node.SetContext(context));
            // Fill the Dual Utilities
            DualUtilityReasoners = new List<DualUtility>();
            UtilityAiBrain.GetOptionNodes().ForEach(node => DualUtilityReasoners.Add(new DualUtility(node, node.GetValue())));
            // Remove ImpossibleDecisionValue Ranks
            DualUtilityReasoners.RemoveAll(reasoner => reasoner.Rank <= 0f);
            // Get max Rank
            int maxRank = DualUtilityReasoners.Max(utility => utility.Rank);
            for (int i = maxRank; i > 0; i--) {
                List<DualUtility> dualUtilities = DualUtilityReasoners.FindAll(utility => utility.Rank == i);
                if (dualUtilities.Count == 0 || dualUtilities.Sum(utility => utility.Utility) <= 0) continue;
                // Calculating Weight
                dualUtilities.ForEach(dualUtility => dualUtility.Weight = dualUtility.Utility / DualUtilityReasoners.Sum(utility => utility.Utility));
                // Rolling probability on weighted random
                LastProbabilityResult = Random.Range(0f, 1f);
                float weightSum = 0f;
                foreach (DualUtility dualUtility in dualUtilities) {
                    weightSum += dualUtility.Weight;
                    if (weightSum >= LastProbabilityResult)
                        return dualUtility;
                }
            }
            return null;
        }
        
    }
}