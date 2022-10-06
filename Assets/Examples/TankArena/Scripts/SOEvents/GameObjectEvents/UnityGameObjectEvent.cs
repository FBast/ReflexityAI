using System;
using UnityEngine;
using UnityEngine.Events;

namespace Examples.TankArena.Scripts.SOEvents.GameObjectEvents {
    [Serializable] public class UnityGameObjectEvent : UnityEvent<GameObject> {}
}