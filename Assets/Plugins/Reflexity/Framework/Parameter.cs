using System;

namespace Plugins.Reflexity.Framework {
	[Serializable]
	public struct Parameter {

		public string Name;
		public string TypeName;

		public Parameter(string name, string typeName) {
			Name = name;
			TypeName = typeName;
		}

	}
}