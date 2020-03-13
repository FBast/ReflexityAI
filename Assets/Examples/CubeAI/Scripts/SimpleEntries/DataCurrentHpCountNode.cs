using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Examples.CubeAI.Scripts.SimpleEntries {
    public class DataCurrentHpCountNode : DataEntryNode {

        protected override int ValueProvider(AbstractAIComponent context) {
            return GetData<GameObject>().GetComponent<CubeEntity>().CurrentHp;
        }
        
    }

    public class DataIsAlive : DataEntryNode {

        protected override int ValueProvider(AbstractAIComponent context) {
            return GetData<GameObject>().GetComponent<CubeEntity>().IsDead ? 0 : 1;
        }
    }
}