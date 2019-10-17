using System.Collections.Generic;
using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Plugins.xNodeUtilityAi.MemoryNodes {
    public class MemoryClearNode : SimpleActionNode {

        public override void Execute(AbstractAIComponent context, AIData aiData) {
            foreach (KeyValuePair<string,Object> keyValuePair in aiData) {
                context.ClearFromMemory(keyValuePair.Key);
            }
        }
    }
}