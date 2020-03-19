using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Examples.CubeAI.Scripts.EntryNodes {
    public class CubeEntityIsDeadNode : EntryBoolNode {
        
        protected override bool ValueProvider(AbstractAIComponent context) {
            return GetData<CubeEntity>().IsDead;
        }
        
    }
}