using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Examples.CubeAI.Scripts.DataActions {
    public class SetTarget : DataActionNode {

        public override void Execute(AbstractAIComponent context, AIData aiData) {
            CubeAIComponent cubeAiComponent = (CubeAIComponent) context;
            cubeAiComponent.CubeEntity.Target = aiData.GetData<GameObject>();
        }
        
    }
}