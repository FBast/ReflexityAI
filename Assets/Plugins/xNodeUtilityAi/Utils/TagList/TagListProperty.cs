using System;
using System.Reflection;
using UnityEngine;

namespace Plugins.xNodeUtilityAi.Utils.TagList {
    public class TagListProperty : PropertyAttribute {
        
        public string[] List { get; }

        public TagListProperty(Type type, string methodName) {
            MethodInfo method = type.GetMethod (methodName);
            if (method != null) {
                List = method.Invoke(null, null) as string[];
            } else {
                Debug.LogError ("No such method " + methodName + " for " + type);
            }
        }

    }
}