using NodeUtilityAi;
using NodeUtilityAi.AbstractNodes;
using NodeUtilityAi.Framework;

namespace Examples.CubeAI.Nodes {
    public class MaxAmmoCountNode : SimpleEntryNode {

        protected override int ValueProvider(AbstractAIComponent context) {
            CubeAIComponent cubeAiComponent = (CubeAIComponent) context;
            return cubeAiComponent.CubeEntity.MaxAmmo;
        }
        
    }
}