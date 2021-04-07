using Examples.TankArena.Scripts.SOEvents.VoidEvents;
using Examples.TankArena.Scripts.SOReferences.FloatReference;
using UnityEngine;

namespace Examples.TankArena.Scripts.Framework {
    public class Timer : MonoBehaviour {

        [Header("SO Events")] 
        public VoidEvent OnTimerFinished;

        [Header("SO References")] 
        public FloatReference MaxTimeReference;
        public FloatReference CurrentTimeReference;

        private bool _isTimeOut;
        
        private void Start() {
            MaxTimeReference.Value = PlayerPrefs.GetInt(Properties.PlayerPrefs.MatchDuration,
                Properties.PlayerPrefsDefault.MatchDuration);
            Time.timeScale = 1;
        }

        private void Update() {
            if (_isTimeOut) return;
            CurrentTimeReference.Value += Time.deltaTime;
            if (CurrentTimeReference.Value < MaxTimeReference.Value) return;
            _isTimeOut = true;
            OnTimerFinished.Raise();
        }

        public void PauseTime(bool pause) {
            Time.timeScale = pause ? 0 : 1;
        }
        
        private void OnDestroy() {
            CurrentTimeReference.Value = 0;
        }
    }
}
