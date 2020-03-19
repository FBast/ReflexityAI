using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Examples.CubeAI.Scripts.EntryNodes {
    public class CubeEntityIsTargetNode : EntryBoolNode {
        
        protected override bool ValueProvider(AbstractAIComponent context) {
            CubeAIComponent cubeAiComponent = (CubeAIComponent) context;
            return cubeAiComponent.CubeEntity.Target == GetData<CubeEntity>();
        }
        
    }
}