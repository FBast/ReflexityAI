using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Examples.CubeAI.Scripts.EntryNodes {
    public class DataIsTarget : EntryBoolNode {
        
        protected override bool ValueProvider(AbstractAIComponent context) {
            GameObject otherCube = GetData<GameObject>();
            CubeAIComponent cubeAiComponent = (CubeAIComponent) context;
            return cubeAiComponent.CubeEntity.Target == otherCube;
        }
        
    }
}