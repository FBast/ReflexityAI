using System;
using System.Linq;
using UnityEngine;
using XNode;

namespace UtilityAI.Nodes {
    public abstract class OptionNode<T> : ExitNode<T> {

        public enum MergeType {
            Average,
            Max,
            Min
        }
        
        public MergeType UtilityMerge;
        [Input] public float Utilities;
        [Input] public int Bonus = 1;
        [Input] public int Multiplier = 1;
        
        public Tuple<float, int> GetValue() {
            NodePort utilityPort = GetInputPort("Utilities");
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
            }
            else {
                utility = Utilities;
            }
            NodePort bonusPort = GetInputPort("Bonus");
            int bonus = bonusPort.IsConnected ? bonusPort.GetInputValues<int>().Sum() : Bonus;
            NodePort multiplierPort = GetInputPort("Multiplier");
            int multiplier = multiplierPort.IsConnected ? multiplierPort.GetInputValues<int>()
                .Aggregate((total, next) => total * next) : Multiplier;
            return new Tuple<float, int>(utility, bonus * multiplier);
        }

    }
}
