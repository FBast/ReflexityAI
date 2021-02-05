using System.Collections.Generic;
using Plugins.ReflexityAI.Framework;
using UnityEngine;

namespace Examples.CubeAI.Scripts {
    public class CubeAI : ReflexityAI {
        
        // External References
        [HideInInspector] public CubeEntity CubeEntity;
        [HideInInspector] public List<CubeEntity> CubeEntities => CubeEntity.CubeEntities;
        
        private void Awake() {
            CubeEntity = GetComponent<CubeEntity>();
        }

    }
}