using System;
using Plugins.Reflexity.Framework;
using UnityEngine;

namespace Examples.TankArena.Scripts.AI {
    [Serializable, CreateAssetMenu(fileName = "TankGraph", menuName = "ReflexityAI/TankGraph")]
    public class TankGraph : AIBrainGraph<TankAI> {}
}