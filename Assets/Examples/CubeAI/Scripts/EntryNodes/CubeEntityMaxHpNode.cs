using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Examples.CubeAI.Scripts.EntryNodes {
    public class CubeEntityMaxHpNode : EntryIntNode {

        protected override int ValueProvider(AbstractAIComponent context) {
            CubeEntity cubeEntity = GetData<CubeEntity>();
            if (cubeEntity == null) 
                Debug.Log("Strangely, cube entity is null");
            return cubeEntity.MaxHp;
        }
        
    }
}