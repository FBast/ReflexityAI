using Examples.TankArena.Scripts.Framework;
using Examples.TankArena.Scripts.SOEvents.VoidEvents;
using Examples.TankArena.Scripts.SOReferences.FloatReference;
using Examples.TankArena.Scripts.Utils;
using UnityEngine;
using PlayerPrefs = UnityEngine.PlayerPrefs;

namespace Examples.TankArena.Scripts.Managers {
	public class GameManager : Singleton<GameManager> {

		[Header("SO Events")] 
		public VoidEvent OnTimerFinished;

		[Header("SO References")] 
		public FloatReference MaxTimeReference;
		public FloatReference CurrentTimeReference;

		private bool _isTimeOut;

		private void Start() {
			MaxTimeReference.Value = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.MatchDuration,
				GlobalProperties.PlayerPrefsDefault.MatchDuration);
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