using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.Framework;

namespace Examples.CubeAI.Scripts {
    public class CubeAIComponent : AbstractAIComponent {
        
        // External References
        public CubeEntity CubeEntity;
        public List<CubeEntity> OthersCubeEntities => GameManager.CubeEntities
            .Where(entity => entity != CubeEntity).ToList();

        private void Awake() {
            CubeEntity = GetComponent<CubeEntity>();
        }

    }
}
