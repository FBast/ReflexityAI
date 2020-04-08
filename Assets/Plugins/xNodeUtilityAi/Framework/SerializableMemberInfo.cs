using System;
using System.Linq;
using System.Reflection;

namespace Plugins.xNodeUtilityAi.Framework {
    public abstract class SerializableMemberInfo {

        public const BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        
        public MemberTypes MemberTypes;
        public string DeclaringTypeName;
        public string Name;
        public string TypeName;
        public string PortName;
        public int Order;
        
        public Type Type => Type.GetType(TypeName);

        protected MemberInfo GetMemberInfo() {
            Type declaringType = Type.GetType(DeclaringTypeName);
            if (declaringType == null) throw new Exception("Cannot find declaring type : " + DeclaringTypeName);
            return declaringType.GetMember(Name, MemberTypes, DefaultBindingFlags).FirstOrDefault();
        }
        
    }
}