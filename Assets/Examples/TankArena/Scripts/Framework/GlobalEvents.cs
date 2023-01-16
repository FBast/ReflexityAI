using System;

namespace Examples.TankArena.Scripts.Framework {
    public static class GlobalActions {
        
        public static Action OnMatchFinished;
        public static Action OnTimerFinished;
        public static Action<string> OnReloadScene;
        public static Action<string> OnUnloadScene;
        public static Action<string> OnLoadScene;

    }
}