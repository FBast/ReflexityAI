using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Examples.CubeAI.Scripts.EntryNodes {
    public class CubeEntityCurrentHpNode : EntryIntNode {

        protected override int ValueProvider(AbstractAIComponent context) {
            return GetData<CubeEntity>().CurrentHp;
        }
        
    }
}