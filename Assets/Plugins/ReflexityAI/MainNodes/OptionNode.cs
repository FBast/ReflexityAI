﻿using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.ReflexityAI.DataNodes;
using Plugins.ReflexityAI.Framework;
using UnityEngine;
using XNode;

namespace Plugins.ReflexityAI.MainNodes {
    [NodeTint(255, 255, 120), NodeWidth(300)]
    public class OptionNode : Node {

        public enum MergeType {
            Average,
            Max,
            Min
        }

        [Input(ShowBackingValue.Never, ConnectionType.Override), Tooltip("Connect to the Data Iterator Node")]
        public DataIteratorNode DataIteratorNode;
        [TextArea, Tooltip("Provide a basic description displayed in the AI Debugger")] public string Description;
        
        [Header("Ranking")] 
        [Input, Tooltip("Connect to each Utility Nodes")] public int Ranks;
        [Tooltip("Average : The rank is calculated using the average of all Utilities\n"
                 + "Max : The rank is calculated using the maximum value of all Utilities\n"
                 + "Min : The rank is calculated using the minimum value of all Utilities")]
        public MergeType Merge = MergeType.Max;
        
        [Header("Weighting")] 
        [Input, Tooltip("Product of the multiplier"), Range(0, 10)] public int Multiplier = 1;
        [Input, Tooltip("Sum of the bonus"), Range(0, 10)] public int Bonus;
        
        [Space] [Input(ShowBackingValue.Never), Tooltip("Connect to each Action Nodes")]
        public ActionNode Actions;
        
        public List<AIOption> GetOptions() {
            List<AIOption> options = new List<AIOption>();
            DataIteratorNode iteratorNode = GetInputPort(nameof(DataIteratorNode)).GetInputValue<DataIteratorNode>();
            if (iteratorNode != null) {
                int collectionSize = iteratorNode.CollectionCount;
                AIBrainGraph brainGraph = (AIBrainGraph) graph;
                while (collectionSize > iteratorNode.Index) {
                    options.Add(new AIOption(this));
                    iteratorNode.Index++;
                    foreach (ICacheable cacheable in brainGraph.GetNodes<ICacheable>()) {
                        cacheable.ClearCache();
                    }
                }
                iteratorNode.Index = 0;
            } else {
                options.Add(new AIOption(this));
            }
            return options;
        }

        public int GetRank() {
            NodePort utilityPort = GetInputPort(nameof(Ranks));
            if (utilityPort.IsConnected) {
                int[] ints = utilityPort.GetInputValues<int>();
                switch (Merge) {
                    case MergeType.Average:
                        return (int) ints.Average();
                    case MergeType.Max:
                        return Mathf.Max(ints);
                    case MergeType.Min:
                        return Mathf.Min(ints);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return Ranks;
        }

        public int GetWeight() {
            NodePort bonusPort = GetInputPort(nameof(Bonus));
            int bonus = bonusPort.IsConnected ? bonusPort.GetInputValues<int>().Sum() : Bonus;
            NodePort multiplierPort = GetInputPort(nameof(Multiplier));
            int multiplier = multiplierPort.IsConnected
                ? multiplierPort.GetInputValues<int>().Aggregate((total, next) => total * next)
                : Multiplier;
            if (bonus == 0) bonus = 1;
            return bonus * multiplier;
        }

        public ActionNode[] GetActions() {
            return GetInputPort(nameof(Actions)).GetInputValues<ActionNode>();
        }

    }
}