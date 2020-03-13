using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Examples.CubeAI.Scripts.CollectionEntries {
    public class OtherCubesNode : CollectionEntryNode {

        protected override List<Object> CollectionProvider(AbstractAIComponent context) {
            CubeAIComponent cubeAiComponent = (CubeAIComponent) context; 
            return new List<Object>(GameManager.Cubes.Where(o => o != cubeAiComponent.gameObject));
        }
        
    }
}