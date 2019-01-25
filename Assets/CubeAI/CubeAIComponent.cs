using UtilityAI;

namespace CubeAI {
    public class CubeAIComponent : AbstractAiComponent<CubeEntity, CubeAIBrain> {
        
        // External References
        private CubeEntity _cubeEntity;
        
        private void Start() {
            _cubeEntity = GetComponent<CubeEntity>();
            InvokeRepeating("ThinkAndAct", 0, 0.2f);
        }

        private void ThinkAndAct() {
            UtilityReasoner utilityReasoner = ChooseAction(_cubeEntity);
            if (utilityReasoner == null) return;
            utilityReasoner.ActionNode.Execute(_cubeEntity);
        }
        
    }
}
