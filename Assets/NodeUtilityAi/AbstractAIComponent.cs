using System;
using System.Collections.Generic;
using System.Linq;
using NodeUtilityAi.Framework;
using NodeUtilityAi.Nodes;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace NodeUtilityAi {
    public class AbstractAIComponent : MonoBehaviour {
        
        public AbstractAIBrain UtilityAiBrain;
        public float LastProbabilityResult;
        public List<AIOption> Options;
        public Dictionary<string, Object> Memory = new Dictionary<string, Object>();
        
        protected AIOption ChooseOption() {
            if (UtilityAiBrain == null) return null;
            UtilityAiBrain.GetNodes<EntryNode>().ForEach(node => node.SetContext(this));
            UtilityAiBrain.GetNodes<ActionNode>().ForEach(node => node.SetContext(this));
            // Fill the Ranked Options
            Options = new List<AIOption>();
            UtilityAiBrain.GetNodes<OptionNode>().ForEach(node => Options.AddRange(node.GetOptions()));
            // Remove ImpossibleDecisionValue Ranks
            Options.RemoveAll(option => option.Rank <= 0f);
            if (Options.Count == 0)
                return null;
            // Get max Rank
            int maxRank = Options.Max(option => option.Rank);
            for (int i = maxRank; i > 0; i--) {
                List<AIOption> options = Options.FindAll(utility => utility.Rank == i);
                if (options.Count == 0 || options.Sum(utility => utility.Utility) <= 0) continue;
                // Calculating Weight
                options.ForEach(dualUtility => dualUtility.Weight = dualUtility.Utility / Options.Sum(utility => utility.Utility));
                // Rolling probability on weighted random
                LastProbabilityResult = Random.Range(0f, 1f);
                float weightSum = 0f;
                foreach (AIOption dualUtility in options) {
                    weightSum += dualUtility.Weight;
                    if (weightSum >= LastProbabilityResult)
                        return dualUtility;
                }
            }
            return null;
        }

        public void SaveToMemory(string dataTag, Object data) {
            if (LoadFromMemory(dataTag) != null)
                throw new Exception("Impossible to save " + dataTag + ", consider using a " + typeof(MemoryCheckNode)
                    + " before using " + typeof(MemoryAccessNode));
            Memory.Add(dataTag, data);
        }

        public TaggedData LoadFromMemory(string dataTag) {
            Object dataToReturn;
            if (!Memory.TryGetValue(dataTag, out dataToReturn)) return null;
            TaggedData taggedData = new TaggedData {DataTag = dataTag, Data = dataToReturn};
            return taggedData;
        }
        
        public bool ClearFromMemory(string dataTag) {
            return Memory.Remove(dataTag);
        }
        
    }

}