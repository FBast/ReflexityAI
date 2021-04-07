using System;
using UnityEngine.Events;

namespace Examples.TankArena.Scripts.SOEvents.StringObjectEvents {
    [Serializable] public class UnityStringObjectEvent : UnityEvent<Tuple<string, object>> {}
}