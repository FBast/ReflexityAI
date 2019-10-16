using NodeUtilityAi;
using NodeUtilityAi.Framework;
using UnityEngine;

namespace Examples.CubeAI {
    public class CubeAIComponent : AbstractAIComponent {
        
        // External References
        [HideInInspector] public CubeEntity CubeEntity;

        private void Awake() {
            CubeEntity = GetComponent<CubeEntity>();
        }

    }
}
