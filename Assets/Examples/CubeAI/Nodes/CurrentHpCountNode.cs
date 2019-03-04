﻿using NodeUtilityAi;
using NodeUtilityAi.Nodes;

namespace Examples.CubeAI.Nodes {
    public class CurrentHpCountNode : SimpleEntryNode {

        protected override int ValueProvider(AbstractAIComponent context) {
            CubeAIComponent cubeAiComponent = (CubeAIComponent) context;
            return cubeAiComponent.CubeEntity.CurrentHp;
        }
        
    }
}