using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;

namespace Examples.CubeAI.Scripts.EntryNodes {
    public class CubeEntityCurrentAmmoNode : EntryIntNode {

        protected override int ValueProvider(AbstractAIComponent context) {
            return GetData<CubeEntity>().CurrentAmmo;
        }
        
    }
}
