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

        private readonly List<MemberInfo> _memberInfos = new List<MemberInfo>();

        private void OnValidate() {
            List<MemberInfo> memberInfos = GetMemberInfos();
            // Add new ports
            foreach (MemberInfo memberInfo in memberInfos.Where(memberInfo => !_memberInfos.Contains(memberInfo))) {
                AddDynamicOutput(memberInfo.FieldType(), ConnectionType.Multiple, TypeConstraint.None, memberInfo.Name);
                _memberInfos.Add(memberInfo);
            }
            // Remove old ports
            foreach (MemberInfo memberInfo in _memberInfos.Where(memberInfo => !memberInfos.Contains(memberInfo))) {
                RemoveDynamicPort(memberInfo.Name);
                _memberInfos.Remove(memberInfo);
            }
        }

        public override object GetValue(NodePort port) {
            return Application.isPlaying
                ? GetFullValue(port.fieldName)
                : GetReflectedValue(port.fieldName);
        }

        public override object GetReflectedValue(string portName) {
            MemberInfo firstOrDefault = GetMemberInfos().FirstOrDefault(info => info.Name == portName);
            if (firstOrDefault != null)
                return new ReflectionData(firstOrDefault.Name, firstOrDefault.FieldType(), null);
            throw new Exception("No reflected data found for " + portName);
        }

        public override object GetFullValue(string portName) {
            MemberInfo firstOrDefault = GetMemberInfos().FirstOrDefault(info => info.Name == portName);
            if (firstOrDefault != null)
                return new ReflectionData(firstOrDefault.Name, firstOrDefault.FieldType(), firstOrDefault.GetValue(Context));
            throw new Exception("No reflected data found for " + portName);
        }
        
        private List<MemberInfo> GetMemberInfos() {
            if (graph is AIBrainGraph brainGraph && brainGraph.ContextType != null) {
                return brainGraph.ContextType.Type.GetMemberInfos().ToList();
            }
            throw new Exception("No brain graph context type found, please select one");
        }
        
    }
}


