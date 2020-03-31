using Plugins.xNodeUtilityAi.Utils;
using XNode;

namespace Plugins.xNodeUtilityAi.Framework {
    [NodeTint(120, 255, 120)]
    public abstract class DataNode : Node {

        public abstract ReflectionData GetReflectedValue(string portName);
        public abstract ReflectionData GetFullValue(string portName);

    }
}