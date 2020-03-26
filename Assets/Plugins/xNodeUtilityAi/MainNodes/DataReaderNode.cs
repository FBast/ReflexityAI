using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils;
using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.MainNodes {
    public class DataReaderNode : DataNode, IContextual {

        public AbstractAIComponent Context { get; set; }
        
        [HideInInspector] public List<MemberInfo> MemberInfos = new List<MemberInfo>();

        private void OnValidate() {
            if (graph is AIBrainGraph brainGraph && brainGraph.ContextType != null) {
                MemberInfo[] memberInfos = brainGraph.ContextType.Type.GetFieldAndProperties();
                ClearDynamicPorts();
                MemberInfos.Clear();
                foreach (MemberInfo memberInfo in memberInfos) {
                    AddDynamicOutput(memberInfo.FieldType(), ConnectionType.Multiple, 
                        TypeConstraint.None, memberInfo.Name);
                    MemberInfos.Add(memberInfo);
                }
            }
        }

        protected override void Init() {
            base.Init();
            OnValidate();
        }

        public override object GetValue(NodePort port) {
            MemberInfo memberInfo = MemberInfos.FirstOrDefault(info => info.Name == port.fieldName);
            if (memberInfo != null) {
                object context = null;
                if (Context != null) {
                    context = memberInfo.GetValue(Context);
                }
                return new Tuple<MemberInfo, object>(memberInfo, context);
            }
            return null;
        }
        
    }
}

