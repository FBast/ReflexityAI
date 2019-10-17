using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;

namespace Examples.CubeAI.Nodes {
    public class HasTarget : SimpleEntryNode {

        protected override int ValueProvider(AbstractAIComponent context) {
            CubeAIComponent cubeAiComponent = (CubeAIComponent) context;
            if (cubeAiComponent.CubeEntity.Target == null) return 0;
            return cubeAiComponent.CubeEntity.Target.GetComponent<CubeEntity>().IsDead ? 0 : 1;
        }
    }
}