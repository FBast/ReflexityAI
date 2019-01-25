using UtilityAI;

namespace CubeAI {
    public class CubeAIComponent : AbstractAiComponent<CubeEntity, CubeAIBrain> {
        
        // External References
        private CubeEntity _cubeEntity;
        
        private void Start() {
            _cubeEntity = GetComponent<CubeEntity>();
            InvokeRepeating("ThinkAndAct", 0, 0.1f);
        }

        private void ThinkAndAct() {
            DualUtility dualUtility = ChooseAction(_cubeEntity);
            if (dualUtility == null) return;
            dualUtility.ActionNode.Execute(_cubeEntity);
        }
        
    }
}
