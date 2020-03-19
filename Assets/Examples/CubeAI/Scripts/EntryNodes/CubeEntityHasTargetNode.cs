using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;

namespace Examples.CubeAI.Scripts.EntryNodes {
    public class CubeEntityHasTargetNode : EntryBoolNode {

        protected override bool ValueProvider(AbstractAIComponent context) {
            CubeEntity cubeEntity = GetData<CubeEntity>();
            return cubeEntity.Target != null && !cubeEntity.Target.IsDead;
        }
        
    }
}