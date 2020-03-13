using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;

namespace Examples.CubeAI.Scripts.SimpleEntries {
    public class MaxAmmoCountNode : SimpleEntryNode {

        protected override int ValueProvider(AbstractAIComponent context) {
            CubeAIComponent cubeAiComponent = (CubeAIComponent) context;
            return cubeAiComponent.CubeEntity.MaxAmmo;
        }
        
    }
}