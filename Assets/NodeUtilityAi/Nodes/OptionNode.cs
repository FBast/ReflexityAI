using System;
using System.Collections.Generic;
using System.Linq;
using NodeUtilityAi.Framework;
using UnityEngine;
using XNode;

namespace NodeUtilityAi.Nodes {
    [NodeTint(255, 255, 120), NodeWidth(300)]
    public class OptionNode : Node {
        
        public enum MergeType {
            Average,
            Max,
            Min
        }

        //TODO-fred mettre en ConnectionType.Multiple
        [Input(ShowBackingValue.Never, ConnectionType.Override)] public CollectionEntryNode Collection;
        [TextArea] public string Description;
        [Input] public float Utilities = 1;
        public MergeType UtilityMerge;
        [Input] public int Multiplier = 1;
        [Input] public int Bonus;
        [Input(ShowBackingValue.Never)] public ActionNode Actions;

        public List<AIOption> GetOptions() {
            List<AIOption> options = new List<AIOption>();
            //TODO-fred revoir pour une recherche sur plusieurs CollectionEntryNodes
            CollectionEntryNode collectionEntryNodes = GetInputPort("Collection").GetInputValue<CollectionEntryNode>();
            List<ActionNode> actionNodes = GetInputPort("Actions").GetInputValues<ActionNode>().ToList();
            if (collectionEntryNodes != null) {
                while (collectionEntryNodes.CollectionCount > collectionEntryNodes.Index) {
                    options.Add(new AIOption(actionNodes, GetUtility(), Description));
                    collectionEntryNodes.Index++;
                }
                collectionEntryNodes.Index = 0;
            }
            else {
                options.Add(new AIOption(actionNodes, GetUtility(), Description));
            }
            return options;
        }

        private Tuple<float, int> GetUtility() {
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
            return new Tuple<float, int>(utility, (bonus + 1) * multiplier);
        }

        
        
    }
}