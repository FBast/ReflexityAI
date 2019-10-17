using System.Collections.Generic;
using NodeUtilityAi.AbstractNodes;
using NodeUtilityAi.Framework;
using UnityEngine;

namespace NodeUtilityAi.MemoryNodes {
    public class MemoryClearNode : SimpleActionNode {

        public override void Execute(AbstractAIComponent context, AIData aiData) {
            foreach (KeyValuePair<string,Object> keyValuePair in aiData) {
                context.ClearFromMemory(keyValuePair.Key);
            }
        }
    }
}