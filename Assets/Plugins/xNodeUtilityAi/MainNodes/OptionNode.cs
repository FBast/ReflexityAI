using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.DataNodes;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.MainNodes {
    [NodeTint(255, 255, 120), NodeWidth(300)]
    public class OptionNode : Node {

        public enum MergeType {
            Average,
            Max,
            Min
        }

        [Input(ShowBackingValue.Never, ConnectionType.Override), Tooltip("Connect to the Data Iterator Node")]
        public DataIteratorNode DataIteratorNode;
        [TextArea, Tooltip("Provide a basic description displayed in the AI Debugger")]
        public string Description;
        
        [Header("Rank")] [Input, Tooltip("Connect to each Utility Nodes")]
        public float Utilities = 1;
        [Tooltip("Average : The rank is calculated using the average of all Utilities\n"
                 + "Max : The rank is calculated using the maximum value of all Utilities\n"
                 + "Min : The rank is calculated using the minimum value of all Utilities")]
        public MergeType UtilityMerge;
        
        [Header("Weight")] [Input, Tooltip("Product of the multiplier")]
        [Range(0, 10)] public int Multiplier = 1;
        [Input, Tooltip("Sum of the bonus"), Range(0, 10)] public int Bonus;
        
        [Space] [Input(ShowBackingValue.Never), Tooltip("Connect to each Action Nodes")]
        public ActionNode Actions;
        
        public List<AIOption> GetOptions() {
            List<AIOption> options = new List<AIOption>();
            DataIteratorNode iteratorNode = GetInputPort(nameof(DataIteratorNode)).GetInputValue<DataIteratorNode>();
            if (iteratorNode != null) {
                int collectionSize = iteratorNode.GetCollection().Count();
                iteratorNode.Index = 0;
                while (collectionSize > iteratorNode.Index) {
                    options.Add(new AIOption(this));
                    iteratorNode.Index++;
                }
            } else {
                options.Add(new AIOption(this));
            }
            return options;
        }

        public float GetRank() {
            NodePort utilityPort = GetInputPort(nameof(Utilities));
            float utility;
            if (utilityPort.IsConnected) {
                float[] floats = utilityPort.GetInputValues<float>();
                switch (UtilityMerge) {
                    case MergeType.Average:
                        utility = floats.Average();
                        break;
                    case MergeType.Max:
                        utility = Mathf.Max(floats);
                        break;
                    case MergeType.Min:
                        utility = Mathf.Min(floats);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            } else {
                utility = Utilities;
            }
            // Utility clamped between 0 and 1
            return Mathf.Clamp(utility, 0f, 1f);
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

    }
}