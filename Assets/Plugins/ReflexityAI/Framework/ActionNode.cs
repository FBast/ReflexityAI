﻿using UnityEngine;
using XNode;

namespace Plugins.ReflexityAI.Framework {
    [NodeTint(212, 53, 53)]
    public abstract class ActionNode : Node {
        
        [Input(ShowBackingValue.Never, ConnectionType.Override)] public Object Data;
        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)] public ActionNode LinkedOption;
        
        public abstract void Execute(object context, object[] parameters);
        public abstract object GetContext();
        public abstract object[] GetParameters();
        
    }
}