using System;
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
        [Tooltip("Robotic : Always pick best option\n" +
                 "Human : Randomize between best options")]
        public ResolutionType OptionsResolution;
        [Tooltip("Cooperative : One option by brain is executed\n" +
                 "Competitive : One option for all brain is executed")]
        public InteractionType MultiBrainInteraction;
        public readonly Dictionary<AIBrainGraph, List<AIOption>> Options = new Dictionary<AIBrainGraph, List<AIOption>>();
        public readonly Dictionary<AIBrainGraph, List<AIOption>> BestOptions = new Dictionary<AIBrainGraph, List<AIOption>>();
        public readonly Dictionary<AIBrainGraph, AIOption> SelectedOptions = new Dictionary<AIBrainGraph, AIOption>();

        private readonly Dictionary<string, object> _memory = new Dictionary<string, object>();
        private readonly Dictionary<string, float> _historic = new Dictionary<string, float>();
        private bool _isThinking;
        private float _timeSinceLastRefresh;

        private void Update() {
            _timeSinceLastRefresh += Time.deltaTime;
            if (_isThinking || _timeSinceLastRefresh <= TimeBetweenRefresh) return;
            StartCoroutine(ThinkAndAct());
            _timeSinceLastRefresh = 0;
        }

        private IEnumerator ThinkAndAct() {
            _isThinking = true;
            Options.Clear();
            foreach (AIBrainGraph aiBrainGraph in UtilityAiBrains.Where(aiBrainGraph => aiBrainGraph != null)) {
                // Calculate all options weight from all brains
                Options.Add(aiBrainGraph, GetOptions(aiBrainGraph));
                yield return null;
            }
            // Fetch best options according to multi brain interaction
            BestOptionsOnWeight(Options, BestOptions);
            // Check if weight are not enough to start execution
            if (IsWeightEnoughForSelection(BestOptions)) {
                SelectedOptions.Clear();
                foreach (KeyValuePair<AIBrainGraph,List<AIOption>> valuePair in Options) {
                    foreach (AIOption aiOption in valuePair.Value) {
                        SelectedOptions.Add(valuePair.Key, aiOption);
                    }
                }
            }
            // Else calculate all options rank from all brains
            else {
                // Remove zero from all utility
                RemoveZeroRankValues(BestOptions);
                // Fetch selected options according to multi brain interaction and options resolution
                SelectOptionsOnRank(BestOptions, SelectedOptions);
            }
            // Order options for display
            foreach (KeyValuePair<AIBrainGraph,List<AIOption>> valuePair in Options.ToList()) {
                Options[valuePair.Key] = valuePair.Value
                    .OrderByDescending(option => option.Weight)
                    .ThenByDescending(option => option.Rank).ToList();
            }
            // Execute selected options
            ExecuteOptions(SelectedOptions);
            _isThinking = false;
        }

        private List<AIOption> GetOptions(AIBrainGraph aiBrainGraph) {
            List<AIOption> aiOptions = new List<AIOption>();
            // Setup Contexts
            aiBrainGraph.GetNodes<IContextual>().ForEach(node => node.Context = this);
            aiBrainGraph.GetNodes<OptionNode>().ForEach(node => aiOptions.AddRange(node.GetOptions()));
            // Order by Weight
            return aiOptions.OrderByDescending(option => option.Weight).ToList();
        }

        private void BestOptionsOnWeight(Dictionary<AIBrainGraph,List<AIOption>> options, Dictionary<AIBrainGraph,List<AIOption>> bestOptions) {
            bestOptions.Clear();
            switch (MultiBrainInteraction) {
                case InteractionType.Cooperative: {
                    // Cooperative then take best weight of each brain
                    foreach (KeyValuePair<AIBrainGraph,List<AIOption>> valuePair in options.Where(pair => pair.Value.Count > 0)) {
                        int maxWeight = valuePair.Value.Max(option => option.Weight);
                        if (maxWeight == 0) continue;
                        bestOptions.Add(valuePair.Key, valuePair.Value.Where(option => option.Weight == maxWeight).ToList());
                    }
                    break;
                }
                case InteractionType.Competitive: {
                    // Competitive then take best weight from all brain
                    int maxWeight = options.Max(pair => pair.Value.Max(option => option.Weight));
                    if (maxWeight == 0) return;
                    foreach (KeyValuePair<AIBrainGraph,List<AIOption>> valuePair in options.Where(pair => pair.Value.Count > 0)) {
                        List<AIOption> aiOptions = valuePair.Value.Where(option => option.Weight == maxWeight).ToList();
                        if (aiOptions.Count > 0) bestOptions.Add(valuePair.Key, aiOptions);
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsWeightEnoughForSelection(Dictionary<AIBrainGraph,List<AIOption>> options) {
            switch (MultiBrainInteraction) {
                case InteractionType.Cooperative:
                    if (options.Any(valuePair => valuePair.Value.Count > 1)) return false; 
                    break;
                case InteractionType.Competitive:
                    if (options.Sum(pair => pair.Value.Count) > 1) return false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
        }
        
        private void RemoveZeroRankValues(Dictionary<AIBrainGraph, List<AIOption>> bestOptions) {
            foreach (AIOption option in bestOptions.SelectMany(valuePair => valuePair.Value.Where(option => option.Rank <= 0))) {
                option.Rank = 0.0001f;
            }
        }
        
        private void SelectOptionsOnRank(Dictionary<AIBrainGraph, List<AIOption>> options, Dictionary<AIBrainGraph, AIOption> selectedOptions) {
            selectedOptions.Clear();
            switch (OptionsResolution) {
                // Calculate probability
                case ResolutionType.Human when MultiBrainInteraction == InteractionType.Competitive: {
                    // Calculate relative probability from all brain options
                    float rndProbability = Random.Range(0f, 1f);
                    float probabilitySum = 0;
                    float rankSum = options.Sum(pair => pair.Value.Sum(option => option.Rank));
                    foreach (KeyValuePair<AIBrainGraph,List<AIOption>> valuePair in options) {
                        foreach (AIOption aiOption in valuePair.Value) {
                            aiOption.Probability = aiOption.Rank / rankSum;
                            probabilitySum += aiOption.Probability;
                            if (selectedOptions.Count == 0 && probabilitySum >= rndProbability) {
                                selectedOptions.Add(valuePair.Key, aiOption);
                            }
                        }
                    }
                    break;
                }
                case ResolutionType.Human when MultiBrainInteraction == InteractionType.Cooperative: {
                    // Calculate relative probability from same brain options
                    foreach (KeyValuePair<AIBrainGraph,List<AIOption>> valuePair in options) {
                        float rndProbability = Random.Range(0f, 1f);
                        float probabilitySum = 0;
                        float rankSum = valuePair.Value.Sum(option => option.Rank);
                        foreach (AIOption aiOption in valuePair.Value) {
                            aiOption.Probability = aiOption.Rank / rankSum;
                            probabilitySum += aiOption.Probability;
                            if (!selectedOptions.ContainsKey(valuePair.Key) && probabilitySum >= rndProbability) {
                                selectedOptions.Add(valuePair.Key, aiOption);
                            }
                        }
                    }
                    break;
                }
                case ResolutionType.Robotic when MultiBrainInteraction == InteractionType.Competitive: {
                    // Calculate absolute probability from all brain options
                    float maxRank = options.Max(pair => pair.Value.Max(option => option.Rank));
                    foreach (KeyValuePair<AIBrainGraph,List<AIOption>> valuePair in options) {
                        foreach (AIOption aiOption in valuePair.Value) {
                            if (SelectedOptions.Count == 0 && aiOption.Rank >= maxRank) {
                                aiOption.Probability = 1;
                                SelectedOptions.Add(valuePair.Key, aiOption);
                            } else {
                                aiOption.Probability = 0;
                            }
                        }
                    }
                    break;
                }
                case ResolutionType.Robotic when MultiBrainInteraction == InteractionType.Cooperative:
                    // Calculate absolute probability from same brain options
                    foreach (KeyValuePair<AIBrainGraph,List<AIOption>> valuePair in options) {
                        foreach (AIOption aiOption in valuePair.Value) {
                            if (!SelectedOptions.ContainsKey(valuePair.Key) && aiOption.Rank >= valuePair.Value.Max(option => option.Rank)) {
                                aiOption.Probability = 1;
                                SelectedOptions.Add(valuePair.Key, aiOption);
                            } else {
                                aiOption.Probability = 0;
                            }
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void ExecuteOptions(Dictionary<AIBrainGraph, AIOption> selectedOptions) {
            foreach (KeyValuePair<AIBrainGraph,AIOption> selectedOption in selectedOptions) {
                selectedOption.Value.ExecuteActions();
            }
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

    public enum ResolutionType {
        Robotic,
        Human
    }
    
    public enum InteractionType {
        Cooperative,
        Competitive
    }

}