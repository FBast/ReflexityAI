using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plugins.xNodeUtilityAi.Utils {
    public static class ReflectionUtils {

        private const BindingFlags _defaultBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        
        public static IEnumerable<MemberInfo> GetFieldAndProperties(this Type type, BindingFlags bindingFlags = _defaultBindingFlags) {
            return type
                .GetFields(bindingFlags)
                .Cast<MemberInfo>()
                .Concat(type.GetProperties(bindingFlags));
        }

        public static Type FieldType(this MemberInfo memberInfo) {
            switch (memberInfo) {
                case FieldInfo fieldInfo:
                    return fieldInfo.FieldType;
                case PropertyInfo propertyInfo:
                    return propertyInfo.PropertyType;
                default:
                    throw new Exception("MemberInfo must be a FieldInfo or PropertyInfo, not a " + memberInfo.MemberType);
            }
        }

        public static object GetValue(this MemberInfo memberInfo, object context) {
            switch (memberInfo) {
                case FieldInfo fieldInfo:
                    return fieldInfo.GetValue(context);
                case PropertyInfo propertyInfo:
                    return propertyInfo.GetValue(context);
                default:
                    throw new Exception("MemberInfo must be a FieldInfo or PropertyInfo, not a " + memberInfo.MemberType);
            }
        }
        
    }
}