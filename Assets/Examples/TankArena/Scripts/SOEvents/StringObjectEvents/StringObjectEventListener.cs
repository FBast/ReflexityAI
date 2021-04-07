using System;

namespace Examples.TankArena.Scripts.SOEvents.StringObjectEvents {
    public class StringObjectEventListener : BaseGameEventListener<Tuple<string, object>, StringObjectEvent, UnityStringObjectEvent> {}
}