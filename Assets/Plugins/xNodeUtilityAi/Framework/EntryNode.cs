using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.Framework {
    [NodeTint(120, 255, 120), NodeWidth(400)]
    public abstract class EntryNode : Node {
        
        [Input(ShowBackingValue.Never)] public Object Data;
        
        protected AbstractAIComponent _context;
        
        public void SetContext(AbstractAIComponent context) {
            _context = context;
        }

        protected T GetData<T>() where T : class {
            return GetInputValue<T>(nameof(Data));
        }

    }
}