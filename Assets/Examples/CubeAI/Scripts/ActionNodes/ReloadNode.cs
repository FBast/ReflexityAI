using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Examples.CubeAI.Scripts.ActionNodes {
    public class ReloadNode : ActionNode {

        public override void Execute(AbstractAIComponent context, Object data) {
            CubeAIComponent cubeAiComponent = (CubeAIComponent) context;
            cubeAiComponent.CubeEntity.Reload();
        }
        
    }
}