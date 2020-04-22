using System.Collections.Generic;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Examples.CubeAI.Scripts {
    public class CubeAIComponent : AbstractAIComponent {
        
        // External References
        [HideInInspector] public CubeEntity CubeEntity;
        public List<CubeEntity> OthersCubeEntities => GameManager.CubeEntities;
        
        private void Awake() {
            CubeEntity = GetComponent<CubeEntity>();
        }

    }
}