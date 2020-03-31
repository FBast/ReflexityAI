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

        private List<MemberInfo> _memberInfos = new List<MemberInfo>();

        private void OnValidate() {
            _memberInfos = GetMemberInfos();
            ClearDynamicPorts();
            foreach (MemberInfo memberInfo in _memberInfos) {
                AddDynamicOutput(memberInfo.FieldType(), ConnectionType.Multiple, TypeConstraint.None, memberInfo.Name);
            }
        }

        protected override void Init() {
            base.Init();
            OnValidate();
        }

        public override object GetValue(NodePort port) {
            return Application.isPlaying
                ? GetFullValue(port.fieldName)
                : GetReflectedValue(port.fieldName);
        }

        public List<MemberInfo> GetMemberInfos() {
            if (graph is AIBrainGraph brainGraph && brainGraph.ContextType != null) {
                return brainGraph.ContextType.Type.GetMemberInfos().ToList();
            }
            throw new Exception("No brain graph context type found, please select one");
        }
        
        public override ReflectionData GetReflectedValue(string portName) {
            MemberInfo firstOrDefault = _memberInfos.FirstOrDefault(info => info.Name == portName);
            if (firstOrDefault != null)
                return new ReflectionData(firstOrDefault.Name, firstOrDefault.FieldType(), null);
            throw new Exception("No reflected data found for " + portName);
        }

        public override ReflectionData GetFullValue(string portName) {
            MemberInfo firstOrDefault = _memberInfos.FirstOrDefault(info => info.Name == portName);
            if (firstOrDefault != null)
                return new ReflectionData(firstOrDefault.Name, firstOrDefault.FieldType(), firstOrDefault.GetValue(Context));
            throw new Exception("No reflected data found for " + portName);
        }
    }


}


