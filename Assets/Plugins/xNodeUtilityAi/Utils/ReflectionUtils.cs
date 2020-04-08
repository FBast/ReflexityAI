using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plugins.xNodeUtilityAi.Utils {
    public static class ReflectionUtils {

        private const BindingFlags _defaultBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        public static IEnumerable<MemberInfo> GetMembers(this Type type, MemberTypes memberTypes) {
            return type.GetMembers(_defaultBindingFlags)
                .Where(info => (info.MemberType & memberTypes) != 0)
                .OrderBy(info => info.MetadataToken);
        }

        public static Type FieldType(this MemberInfo memberInfo) {
            switch (memberInfo) {
                case FieldInfo fieldInfo:
                    return fieldInfo.FieldType;
                case PropertyInfo propertyInfo:
                    return propertyInfo.PropertyType;
                case MethodInfo methodInfo:
                    return methodInfo.ReturnType;
                case EventInfo eventInfo:
                    return eventInfo.EventHandlerType;
                default:
                    throw new Exception("MemberInfo of type " + memberInfo.MemberType + " is not supported");
            }
        }

        public static bool IsSameOrSubclass(this Type potentialDescendant, Type potentialBase) {
            return potentialDescendant.IsSubclassOf(potentialBase) || potentialDescendant == potentialBase;
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