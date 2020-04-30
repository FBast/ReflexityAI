using System.Collections.Generic;
using Plugins.ReflexityAI.Framework;
using UnityEngine;

namespace Examples.CubeAI.Scripts {
    public class CubeAI : ReflexityAI {
        
        // External References
        [HideInInspector] public CubeEntity CubeEntity;
        public List<CubeEntity> OthersCubeEntities => GameManager.CubeEntities;
        
        private void Awake() {
            CubeEntity = GetComponent<CubeEntity>();
        }

    }
}