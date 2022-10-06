using System;

namespace Examples.TankArena.Scripts.SOReferences {
    /// <summary>
    /// Reference Class.
    /// </summary>
    [Serializable]
    public abstract class Reference
    {
    }

    /// <summary>
    /// Reference Class.
    /// </summary>
    [Serializable]
    public class Reference<T, G> : Reference where G : Variable<T>
    {
        public G Variable;

        public Reference() { }

        public T Value
        {
            get { return Variable.Value; }
            set { Variable.Value = value; }
        }

        public static implicit operator T(Reference<T, G> Reference)
        {
            return Reference.Value;
        }

        public static implicit operator Reference<T, G>(T Value)
        {
            return new Reference<T, G>();
        }
    }
}