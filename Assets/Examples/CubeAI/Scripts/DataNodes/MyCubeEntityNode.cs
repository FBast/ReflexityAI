using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils;

namespace Examples.CubeAI.Scripts.DataNodes {
    public class MyCubeEntityNode : SimpleDataNode {

        protected override object ValueProvider(AbstractAIComponent context) {
            CubeAIComponent cubeAiComponent = (CubeAIComponent) context;
            return cubeAiComponent.CubeEntity;
        }

        public override object GetReflectedValue(string portName) {
            throw new System.NotImplementedException();
        }

        public override object GetFullValue(string portName) {
            throw new System.NotImplementedException();
        }
    }
}