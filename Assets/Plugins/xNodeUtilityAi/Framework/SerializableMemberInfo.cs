using System;
using System.Linq;
using System.Reflection;
using Plugins.xNodeUtilityAi.Utils;

namespace Plugins.xNodeUtilityAi.Framework {
    [Serializable]
    public class SerializableMemberInfo {

        public const BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        
        public MemberTypes MemberTypes;
        public string DeclaringTypeName;
        public string Name;
        public string TypeName;
        public string PortName;
        public int Order;
        public bool IsIteratable;
        public bool IsPrimitive;
        
        public Type Type => Type.GetType(TypeName);

        protected MemberInfo GetMemberInfo() {
            Type declaringType = Type.GetType(DeclaringTypeName);
            if (declaringType == null) throw new Exception("Cannot find declaring type : " + DeclaringTypeName);
            return declaringType.GetMember(Name, MemberTypes, DefaultBindingFlags).FirstOrDefault();
        }

        public virtual object GetEditorValue() {
            return IsPrimitive ? null : 
                new Tuple<string, Type, object>(Name, Type, null);
        }

        public virtual object GetRuntimeValue(object context) {
            MemberInfo memberInfo = GetMemberInfo();
            if (context == null) throw new Exception("Cannot get Runtime value if context is null");
            return IsPrimitive ? memberInfo.GetValue(context) : 
                new Tuple<string, Type, object>(Name, Type, memberInfo.GetValue(context));
        }

    }
}