using Examples.TankArena.Scripts.Framework;
using Examples.TankArena.Scripts.Utils;
using UnityEngine;
using PlayerPrefs = UnityEngine.PlayerPrefs;

namespace Examples.TankArena.Scripts.Managers {
	public class GameManager : Singleton<GameManager> {

		public int PointPerKill => PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.PointsPerKill,
			GlobalProperties.PlayerPrefsDefault.PointsPerKill);
		public int PointPerBonus => PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.PointsPerBonus,
			GlobalProperties.PlayerPrefsDefault.PointsPerBonus);
		public int PointPerTeamKill => PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.PointsPerTeamKill,
			GlobalProperties.PlayerPrefsDefault.PointsPerTeamKill);
		public int PointPerVictory => PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.PointsForVictory,
			GlobalProperties.PlayerPrefsDefault.PointsForVictory);
		
		private bool _isTimeOut;

		private void Start() {
			GlobalFields.MaxTime = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.MatchDuration,
				GlobalProperties.PlayerPrefsDefault.MatchDuration);
			Time.timeScale = 1;
		}

		private void Update() {
			if (_isTimeOut) return;
			GlobalFields.CurrentTime += Time.deltaTime;
			if (GlobalFields.CurrentTime < GlobalFields.MaxTime) return;
			_isTimeOut = true;
			GlobalActions.OnTimerFinished.Invoke();
		}

		public void PauseTime(bool pause) {
			Time.timeScale = pause ? 0 : 1;
		}
        
		private void OnDestroy() {
			GlobalFields.CurrentTime = 0;
		}
		
	}
}