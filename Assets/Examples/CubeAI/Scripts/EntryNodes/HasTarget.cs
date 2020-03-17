using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;

namespace Examples.CubeAI.Scripts.EntryNodes {
    public class HasTarget : EntryBoolNode {

        protected override bool ValueProvider(AbstractAIComponent context) {
            CubeAIComponent cubeAiComponent = (CubeAIComponent) context;
            return cubeAiComponent.CubeEntity.Target != null && !cubeAiComponent.CubeEntity.Target.GetComponent<CubeEntity>().IsDead;
        }
        
    }
}