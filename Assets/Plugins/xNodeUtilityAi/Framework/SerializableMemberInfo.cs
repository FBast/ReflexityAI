using System;
using System.Linq;
using System.Reflection;
using Plugins.xNodeUtilityAi.Utils;

namespace Plugins.xNodeUtilityAi.Framework {
    [Serializable]
    public class SerializableMemberInfo {

        public string DeclaringTypeName;
        public string FieldName;
        public string TypeName;
        public bool IsPrimitive;

        public SerializableMemberInfo(MemberInfo memberInfo) {
            DeclaringTypeName = memberInfo.DeclaringType?.AssemblyQualifiedName;
            FieldName = memberInfo.Name;
            TypeName = memberInfo.FieldType().AssemblyQualifiedName;
            IsPrimitive = memberInfo.FieldType().IsPrimitive;
        }

        public MemberInfo ToMemberInfo() {
            Type declaringType = Type.GetType(DeclaringTypeName);
            if (declaringType == null) throw new Exception("Cannot find declaring type : " + DeclaringTypeName);
            return declaringType.GetMember(FieldName).FirstOrDefault();
        }

        public object GetEditorValue() {
            MemberInfo memberInfo = ToMemberInfo();
            return IsPrimitive ? null : 
                new Tuple<string, Type, object>(FieldName, memberInfo.FieldType(), null);
        }

        public object GetRuntimeValue(object context) {
            MemberInfo memberInfo = ToMemberInfo();
            return IsPrimitive ? memberInfo.GetValue(context) : 
                new Tuple<string, Type, object>(FieldName, memberInfo.FieldType(), memberInfo.GetValue(context));
        }

    }
}