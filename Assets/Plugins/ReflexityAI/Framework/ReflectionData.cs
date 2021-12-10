using System;

namespace Plugins.Reflexity.Framework {
	public struct ReflectionData {

		public Type Type;
		public object Value;
		public bool FromIteration;
        
		public ReflectionData(Type type, object value, bool fromIteration = false) {
			Type = type;
			Value = value;
			FromIteration = fromIteration;
		}

	}
}