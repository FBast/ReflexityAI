using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Examples.CubeAI.Scripts.DataEntries {
    public class DataIsDead : DataEntryNode {
        
        protected override int ValueProvider(AbstractAIComponent context) {
            GameObject otherCube = GetData<GameObject>();
            return otherCube.GetComponent<CubeEntity>().IsDead ? 1 : 0;
        }
        
    }
}