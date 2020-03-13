using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Examples.CubeAI.Scripts.SimpleEntries {
    public class DataMaxHpCountNode : DataEntryNode {

        protected override int ValueProvider(AbstractAIComponent context) {
            return GetData<GameObject>().GetComponent<CubeEntity>().MaxHp;
        }
        
    }
}