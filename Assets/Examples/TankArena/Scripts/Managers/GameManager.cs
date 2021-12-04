using System.Collections.Generic;
using System.Linq;
using Examples.TankArena.Scripts.Data;
using Examples.TankArena.Scripts.Framework;
using Examples.TankArena.Scripts.SOEvents.VoidEvents;
using Examples.TankArena.Scripts.SOReferences.FloatReference;
using Examples.TankArena.Scripts.Utils;
using UnityEditor;
using UnityEngine;

namespace Examples.TankArena.Scripts.Managers {
	public class GameManager : Singleton<GameManager> {
		
		[Header("Data")]
		public List<Material> SkyBoxes;
		
		[Header("SO Events")] 
		public VoidEvent OnTimerFinished;

		[Header("SO References")] 
		public FloatReference MaxTimeReference;
		public FloatReference CurrentTimeReference;

		private bool _isTimeOut;
		
		public List<TankSetting> TankSettings { get; private set; }
		
		private void Awake() {
			TankSettings = AssetDatabase.FindAssets("t:TankSetting", new[] {"Assets"})
					.Select(AssetDatabase.GUIDToAssetPath)
					.Select(AssetDatabase.LoadAssetAtPath<TankSetting>)
					.ToList();
		}

		private void Start() {
			if (SkyBoxes.Count > 0) RenderSettings.skybox = SkyBoxes[Random.Range(0, SkyBoxes.Count)];
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