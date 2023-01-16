using System;
using Examples.TankArena.Scripts.Managers;

namespace Examples.TankArena.Scripts.Framework {
    public static class GlobalActions {
        
        public static Action OnMatchFinished;
        public static Action OnTimerFinished;
        public static readonly Action<string> OnReloadScene = delegate(string scene) {
            CustomSceneManager.Instance.ReloadScene(scene);
        };
        public static readonly Action<string> OnUnloadScene = delegate(string scene) {
            CustomSceneManager.Instance.UnloadScene(scene);
        };
        public static readonly Action<string> OnLoadScene = delegate(string scene) {
            CustomSceneManager.Instance.LoadScene(scene);
        };

    }
}