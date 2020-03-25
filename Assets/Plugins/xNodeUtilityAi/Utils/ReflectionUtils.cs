using System;
using System.Linq;
using System.Reflection;

namespace Plugins.xNodeUtilityAi.Utils {
    public static class ReflectionUtils {

        private const BindingFlags _defaultBindingFlags = BindingFlags.Public | BindingFlags.Instance;
        
        public static MemberInfo[] GetFieldAndProperties(this Type type, BindingFlags bindingFlags = _defaultBindingFlags) {
            return type.GetFields(bindingFlags).Cast<MemberInfo>()
                .Concat(type.GetProperties(bindingFlags)).ToArray();
        }

        public static Type FieldType(this MemberInfo memberInfo) {
            switch (memberInfo.MemberType) {
                case MemberTypes.Field:
                    return ((FieldInfo) memberInfo).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo) memberInfo).PropertyType;
                default:
                    throw new NotSupportedException();
            }
        }

        public static object GetValue(this MemberInfo memberInfo, object context) {
            switch (memberInfo.MemberType) {
                case MemberTypes.Field:
                    return ((FieldInfo) memberInfo).GetValue(context);
                case MemberTypes.Property:
                    return ((PropertyInfo) memberInfo).GetValue(context);
                default:
                    throw new NotSupportedException();
            }
        }
        
    }
}