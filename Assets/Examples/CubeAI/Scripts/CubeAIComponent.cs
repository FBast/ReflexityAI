using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Examples.CubeAI.Scripts {
    public class CubeAIComponent : AbstractAIComponent {
        
        // External References
        [HideInInspector] public CubeEntity CubeEntity;
        public List<CubeEntity> OthersCubeEntities => GameManager.CubeEntities
            .Where(entity => entity != CubeEntity).ToList();
        public int ExempleOfInt;
        public bool ExempleOfBool;
        
        private void Awake() {
            CubeEntity = GetComponent<CubeEntity>();
        }

    }
}
