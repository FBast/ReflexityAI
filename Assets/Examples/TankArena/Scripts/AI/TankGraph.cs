using System;
using Plugins.ReflexityAI.Framework;
using UnityEngine;

namespace Examples.TankArena.Scripts.AI {
    [Serializable, CreateAssetMenu(fileName = "TankGraph", menuName = "ReflexityAI/TankGraph")]
    public class TankGraph : AIBrainGraph<TankAI> {}
}