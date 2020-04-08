using System;
using System.Collections.Generic;
using System.Reflection;

namespace Plugins.xNodeUtilityAi.Framework {
    [Serializable]
    public class SerializableMethodInfo : SerializableMemberInfo {
        
        public List<Parameter> Parameters = new List<Parameter>();

        public SerializableMethodInfo(MethodInfo methodInfo) {
            MemberTypes = methodInfo.MemberType;
            DeclaringTypeName = methodInfo.DeclaringType?.AssemblyQualifiedName;
            Name = methodInfo.Name;
            Type returnType = methodInfo.ReturnType;
            TypeName = returnType.AssemblyQualifiedName;
            PortName = methodInfo.Name + " (" + methodInfo.MemberType + ")";
            Order = methodInfo.MetadataToken;
            foreach (ParameterInfo parameterInfo in methodInfo.GetParameters()) {
                Parameters.Add(new Parameter(parameterInfo.Name, parameterInfo.ParameterType.AssemblyQualifiedName));
            }
        }

        public void Invoke(object context, object[] parameters) {
            MethodInfo methodInfo = (MethodInfo) GetMemberInfo();
            methodInfo.Invoke(context, parameters);
        }

        [Serializable]
        public struct Parameter {

            public string Name;
            public string TypeName;

            public Parameter(string name, string typeName) {
                Name = name;
                TypeName = typeName;
            }

        }
        
    }
}