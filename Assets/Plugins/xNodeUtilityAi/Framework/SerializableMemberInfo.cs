using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plugins.xNodeUtilityAi.Utils;

namespace Plugins.xNodeUtilityAi.Framework {
    [Serializable]
    public class SerializableMemberInfo {

        public MemberTypes MemberType;
        public string DeclaringTypeName;
        public string Name;
        public string PortName;
        public string MainTypeName;
        public List<(string name, string type)> Parameters = new List<(string name, string type)>();
        public bool IsPrimitive;
        public bool IsIteratable;
        public int Order;

        public Type MainType => Type.GetType(MainTypeName);

        public SerializableMemberInfo(MemberInfo memberInfo) {
            MemberType = memberInfo.MemberType;
            DeclaringTypeName = memberInfo.DeclaringType?.AssemblyQualifiedName;
            Name = memberInfo.Name;
            PortName = memberInfo.Name + " (" + memberInfo.MemberType + ")";
            Order = memberInfo.MetadataToken;
            switch (memberInfo) {
                case FieldInfo fieldInfo: {
                    Type fieldType = fieldInfo.FieldType;
                    MainTypeName = fieldType.AssemblyQualifiedName;
                    IsPrimitive = fieldType.IsPrimitive;
                    if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>)) {
                        IsIteratable = true;
                    }
                    break;
                }
                case PropertyInfo propertyInfo: {
                    Type fieldType = propertyInfo.PropertyType;
                    MainTypeName = fieldType.AssemblyQualifiedName;
                    IsPrimitive = fieldType.IsPrimitive;
                    if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>)) {
                        IsIteratable = true;
                    }
                    break;
                }
                case MethodInfo methodInfo: {
                    MainTypeName = methodInfo.ReturnType.AssemblyQualifiedName;
                    foreach (ParameterInfo parameterInfo in methodInfo.GetParameters()) {
                        Parameters.Add((parameterInfo.Name, parameterInfo.ParameterType.AssemblyQualifiedName));
                    }
                    break;
                }
                case EventInfo eventInfo:
                    break;
                default:
                    throw new Exception("Cannot serialize " + memberInfo.MemberType);
            }
        }

        /// <summary>
        /// Return a limited value with name and type
        /// </summary>
        /// <returns></returns>
        public object GetEditorValue() {
            return IsPrimitive ? null : 
                new Tuple<string, Type, object>(Name, MainType, null);
        }

        /// <summary>
        /// Return a complete value with name, type and object value
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public object GetRuntimeValue(object context) {
            MemberInfo memberInfo = ToMemberInfo();
            if (context == null) throw new Exception("Cannot get Runtime value if context is null");
            return IsPrimitive ? memberInfo.GetValue(context) : 
                new Tuple<string, Type, object>(Name, MainType, memberInfo.GetValue(context));
        }
        
        private MemberInfo ToMemberInfo() {
            Type declaringType = Type.GetType(DeclaringTypeName);
            if (declaringType == null) throw new Exception("Cannot find declaring type : " + DeclaringTypeName);
            return declaringType.GetMember(Name).FirstOrDefault();
        }

    }
}