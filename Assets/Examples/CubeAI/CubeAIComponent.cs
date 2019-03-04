using NodeUtilityAi;
using NodeUtilityAi.Framework;

namespace Examples.CubeAI {
    public class CubeAIComponent : AbstractAIComponent {
        
        // External References
        public CubeEntity CubeEntity;

        private void Awake() {
            CubeEntity = GetComponent<CubeEntity>();
        }

        private void Start() {
            InvokeRepeating("ThinkAndAct", 0, 0.1f);
        }

        private void ThinkAndAct() {
            AIOption option = ChooseOption();
            if (option == null) return;
            option.ExecuteActions(this);
        }
        
    }
}
