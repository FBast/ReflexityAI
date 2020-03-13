using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;

namespace Examples.CubeAI.Scripts.SimpleEntries {
    public class IsFullLife : SimpleEntryNode {
        
        protected override int ValueProvider(AbstractAIComponent context) {
            CubeAIComponent cubeAiComponent = (CubeAIComponent) context;
            return cubeAiComponent.CubeEntity.CurrentHp == cubeAiComponent.CubeEntity.MaxHp ? 1 : 0;
        }
        
    }
}