using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Examples.CubeAI.Scripts.DataNodes {
    public class OtherCubeEntitiesNode : DataCollectionNode {

        protected override List<Object> CollectionProvider(AbstractAIComponent context) {
            CubeAIComponent cubeAiComponent = (CubeAIComponent) context;
            return new List<Object>(GameManager.CubeEntities.Where(entity => entity != cubeAiComponent.CubeEntity));
        }
        
    }
}