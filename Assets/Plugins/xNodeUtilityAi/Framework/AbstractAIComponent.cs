using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.MainNodes;
using Plugins.xNodeUtilityAi.MemoryNodes;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Plugins.xNodeUtilityAi.Framework {
    public class AbstractAIComponent : MonoBehaviour {
        
        [Range(0.1f, 5f)] public float TimeBetweenRefresh = 0.5f;
        public bool AlwaysPickBestChoice;
        public List<AIBrain> UtilityAiBrains;

        public readonly Dictionary<AIBrain, List<AIOption>> Options = new Dictionary<AIBrain, List<AIOption>>();
        public Dictionary<AIBrain, AIOption> SelectedOptions = new Dictionary<AIBrain, AIOption>();
        
        private readonly Dictionary<string, Object> _memory = new Dictionary<string, Object>();
        private float _lastProbabilityResult;
        private bool _isThinking;
        private float _timeSinceLastRefresh;

        private void Update() {
            _timeSinceLastRefresh += Time.deltaTime;
            if (_isThinking || _timeSinceLastRefresh <= TimeBetweenRefresh) return;
            StartCoroutine(ThinkAndAct());
            _timeSinceLastRefresh = 0;
        }

        IEnumerator ThinkAndAct() {
            _isThinking = true;
            foreach (AIBrain utilityAiBrain in UtilityAiBrains) {
                CalculateOptions(utilityAiBrain);
                yield return null;
                AIOption aiOption = ChooseOption(utilityAiBrain);
                if (aiOption == null) continue;
                if (SelectedOptions.ContainsKey(utilityAiBrain)) {
                    SelectedOptions[utilityAiBrain] = aiOption;
                } else {
                    SelectedOptions.Add(utilityAiBrain, aiOption);
                }
                aiOption.ExecuteActions(this);
                yield return null;
            }
            _isThinking = false;
        }

        private void CalculateOptions(AIBrain utilityAiBrain) {
            if (utilityAiBrain == null) return;
            // Setup Contexts
            utilityAiBrain.GetNodes<EntryNode>().ForEach(node => node.SetContext(this));
            utilityAiBrain.GetNodes<ActionNode>().ForEach(node => node.SetContext(this));
            // Add the brain to the option dictionary
            if (Options.ContainsKey(utilityAiBrain)) {
                Options[utilityAiBrain].Clear();
            }
            else {
                Options.Add(utilityAiBrain, new List<AIOption>());
            }
            utilityAiBrain.GetNodes<OptionNode>().ForEach(node => Options[utilityAiBrain]
                .AddRange(node.GetOptions()));
            // Return if no option found
            if (Options[utilityAiBrain].Count == 0) return;
            // Calculate Probability
            foreach (AIOption aiOption in Options[utilityAiBrain]) {
                aiOption.Probability = aiOption.Utility / Options[utilityAiBrain]
                                           .Where(option => option.Weight == aiOption.Weight)
                                           .Sum(option => option.Utility);
            }
            // Order by Weight then Utility
            Options[utilityAiBrain] = Options[utilityAiBrain].OrderByDescending(option => option.Weight).ThenByDescending(option => option.Utility).ToList();
        }

        private AIOption ChooseOption(AIBrain aiBrain) {
            // Calcul maxWeight and return null if equal to zero
            int maxWeight = Options[aiBrain].Max(option => option.Weight);
            if (maxWeight == 0) return null;
            // Returning best option for no random
            if (AlwaysPickBestChoice) {
                return Options[aiBrain].FirstOrDefault();
            }
            // Rolling probability on weighted random
            _lastProbabilityResult = Random.Range(0f, 1f);
            float probabilitySum = 0f;
            foreach (AIOption dualUtility in Options[aiBrain].FindAll(option => option.Weight == maxWeight)) {
                probabilitySum += dualUtility.Probability;
                if (probabilitySum >= _lastProbabilityResult)
                    return dualUtility;
            }
            return null;
        }

        public void SaveToMemory(string dataTag, Object data) {
            if (LoadFromMemory(dataTag) != null)
                throw new Exception("Impossible to save " + dataTag + ", consider using a " + typeof(MemoryCheckNode)
                    + " before using " + typeof(MemoryAccessNode));
            _memory.Add(dataTag, data);
        }

        public TaggedData LoadFromMemory(string dataTag) {
            Object dataToReturn;
            if (!_memory.TryGetValue(dataTag, out dataToReturn)) return null;
            TaggedData taggedData = new TaggedData {DataTag = dataTag, Data = dataToReturn};
            return taggedData;
        }
        
        public bool ClearFromMemory(string dataTag) {
            return _memory.Remove(dataTag);
        }
        
    }
}