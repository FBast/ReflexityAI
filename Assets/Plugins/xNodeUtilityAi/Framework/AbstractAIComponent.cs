using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.MainNodes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Plugins.xNodeUtilityAi.Framework {
    public class AbstractAIComponent : MonoBehaviour {

        [Tooltip("Brains used by the AI")]
        public List<AIBrainGraph> UtilityAiBrains;
        [Tooltip("Time between each AI evaluations")]
        [Range(0.1f, 5f)] public float TimeBetweenRefresh = 0.5f;
        [Tooltip("No randomisation between best options")]
        public bool AlwaysPickBestChoice;
        [Tooltip("Cooperative : One option by brain is executed\n" +
                 "Competitive : One option for all brain is executed")]
        public BrainType BrainType;
        public readonly Dictionary<AIBrainGraph, List<AIOption>> Options = new Dictionary<AIBrainGraph, List<AIOption>>();
        public Dictionary<AIBrainGraph, AIOption> SelectedOptions = new Dictionary<AIBrainGraph, AIOption>();

        private readonly Dictionary<string, object> _memory = new Dictionary<string, object>();
        private readonly Dictionary<string, float> _historic = new Dictionary<string, float>();
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
            foreach (AIBrainGraph aiBrainGraph in UtilityAiBrains) {
                if (aiBrainGraph == null) continue;
                CalculateOptions(aiBrainGraph);
                yield return null;
                AIOption aiOption = ChooseOption(aiBrainGraph);
                if (aiOption == null) continue;
                if (SelectedOptions.ContainsKey(aiBrainGraph)) {
                    SelectedOptions[aiBrainGraph] = aiOption;
                } else {
                    SelectedOptions.Add(aiBrainGraph, aiOption);
                }
                aiOption.ExecuteActions(this);
                yield return null;
            }
            _isThinking = false;
        }

        private void CalculateOptions(AIBrainGraph aiBrainGraph) {
            // Setup Contexts
            aiBrainGraph.GetNodes<IContextual>().ForEach(node => node.Context = this);
            // Add the brain to the option dictionary
            if (Options.ContainsKey(aiBrainGraph)) {
                Options[aiBrainGraph].Clear();
            }
            else {
                Options.Add(aiBrainGraph, new List<AIOption>());
            }
            aiBrainGraph.GetNodes<OptionNode>().ForEach(node => Options[aiBrainGraph]
                .AddRange(node.GetOptions()));
            // Return if no option found
            if (Options[aiBrainGraph].Count == 0) return;
            // Calculate Probability
            foreach (AIOption aiOption in Options[aiBrainGraph]) {
                aiOption.Probability = aiOption.Utility / Options[aiBrainGraph]
                                           .Where(option => option.Weight == aiOption.Weight)
                                           .Sum(option => option.Utility);
            }
            // Order by Weight then Utility
            Options[aiBrainGraph] = Options[aiBrainGraph].OrderByDescending(option => option.Weight).ThenByDescending(option => option.Utility).ToList();
        }

        private AIOption ChooseOption(AIBrainGraph aiBrainGraph) {
            // Calcul maxWeight and return null if equal to zero
            int maxWeight = Options[aiBrainGraph].Max(option => option.Weight);
            if (maxWeight == 0) return null;
            // Returning best option for no random
            if (AlwaysPickBestChoice) {
                return Options[aiBrainGraph].FirstOrDefault();
            }
            // Rolling probability on weighted random
            _lastProbabilityResult = Random.Range(0f, 1f);
            float probabilitySum = 0f;
            foreach (AIOption dualUtility in Options[aiBrainGraph].FindAll(option => option.Weight == maxWeight)) {
                probabilitySum += dualUtility.Probability;
                if (probabilitySum >= _lastProbabilityResult)
                    return dualUtility;
            }
            return null;
        }

        // Memory

        public void SaveInMemory(string memoryTag, object data) {
            if (_memory.ContainsKey(memoryTag)) _memory[memoryTag] = data;
            else _memory.Add(memoryTag, data);
        }

        public object LoadFromMemory(string memoryTag) {
            return _memory.ContainsKey(memoryTag) ? _memory[memoryTag] : null;
        }
        
        public void ClearFromMemory(string memoryTag) {
            _memory.Remove(memoryTag);
        }
        
        // Historic

        public void SaveInHistoric(string historicTag) {
            if (_historic.ContainsKey(historicTag)) _historic[historicTag] = Time.realtimeSinceStartup;
            else _historic.Add(historicTag, Time.realtimeSinceStartup);
        }

        public float HistoricTime(string historicTag) {
            return _historic.ContainsKey(historicTag) ? _historic[historicTag] : 0;
        }

    }

    public enum BrainType {
        Cooperative,
        Competitive
    }

}