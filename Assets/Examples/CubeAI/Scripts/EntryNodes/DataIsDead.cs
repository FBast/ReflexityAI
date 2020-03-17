using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Examples.CubeAI.Scripts.EntryNodes {
    public class DataIsDead : EntryBoolNode {
        
        protected override bool ValueProvider(AbstractAIComponent context) {
            GameObject otherCube = GetData<GameObject>();
            return otherCube.GetComponent<CubeEntity>().IsDead;
        }
        
    }
}