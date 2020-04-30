using System;
using System.Reflection;
using UnityEngine;

namespace Plugins.ReflexityAI.Utils.TagList {
    public class DropdownList : PropertyAttribute {

        public string[] List { get; }

        public DropdownList(Type type, string staticMethodName) {
            MethodInfo method = type.GetMethod(staticMethodName);
            if (method != null) {
                List = method.Invoke(null, null) as string[];
            } else {
                Debug.LogError("No such static method " + staticMethodName + " for " + type);
            }
        }

        public DropdownList(object context, MemberTypes memberTypes, string memberName) {
            switch (memberTypes) {
                case MemberTypes.Field:
                    FieldInfo field = context.GetType().GetField(memberName);
                    if (field != null) {
                        List = field.GetValue(context) as string[];
                    } else {
                        Debug.LogError("No such field " + memberName + " for " + context.GetType());
                    }
                    break;
                case MemberTypes.Method:
                    MethodInfo method = context.GetType().GetMethod(memberName);
                    if (method != null) {
                        List = method.Invoke(context, null) as string[];
                    } else {
                        Debug.LogError("No such method " + memberName + " for " + context.GetType());
                    }
                    break;
                case MemberTypes.Property:
                    PropertyInfo property = context.GetType().GetProperty(memberName);
                    if (property != null) {
                        List = property.GetValue(context) as string[];
                    } else {
                        Debug.LogError("No such property " + memberName + " for " + context.GetType());
                    }
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
        
    }
}