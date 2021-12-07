using System.Collections.Generic;
using System.Linq;
using Examples.TankArena.Scripts.Framework;
using Examples.TankArena.Scripts.SOEvents.StringEvents;
using Examples.TankArena.Scripts.SOReferences.MatchReference;
using UnityEngine;

namespace Examples.TankArena.Scripts.UI {
    public class MatchEndUI : MonoBehaviour {

        [Header("Prefabs")] 
        public GameObject TeamStatLine;

        [Header("Internal References")] 
        public Transform StatsContent;
        
        [Header("SO References")] 
        public MatchReference CurrentMatchReference;

        [Header("SO Events")] 
        public StringEvent OnReloadScene;
  
        private void OnEnable() {
            foreach (KeyValuePair<Team,Stats> teamStat in CurrentMatchReference.Value.TeamStats) {
                if (teamStat.Value.TankLeft > 0 && CurrentMatchReference.Value.TeamStats.All(pair => pair.Value.TankLeft == 0))
                    teamStat.Value.VictoryPoints = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.PointsForVictory, GlobalProperties.PlayerPrefsDefault.PointsForVictory);
                teamStat.Value.TotalPoints += teamStat.Value.TeamKill * PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.PointsPerTeamKill, GlobalProperties.PlayerPrefsDefault.PointsPerTeamKill);
                teamStat.Value.TotalPoints += teamStat.Value.KillCount * PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.PointsPerKill, GlobalProperties.PlayerPrefsDefault.PointsPerKill);
                teamStat.Value.TotalPoints += teamStat.Value.BonusCount * PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.PointsPerBonus, GlobalProperties.PlayerPrefsDefault.PointsPerBonus);
                teamStat.Value.TotalPoints += teamStat.Value.VictoryPoints;
            }
            // Display stats
            foreach (KeyValuePair<Team,Stats> teamStat in CurrentMatchReference.Value.TeamStats.OrderByDescending(pair => pair.Value.TotalPoints)) {
                TeamStatLineUI teamStatLineUi = Instantiate(TeamStatLine, StatsContent.transform).GetComponent<TeamStatLineUI>();
                teamStatLineUi.TeamNameText.text = teamStat.Key.TeamName;
                teamStatLineUi.TeamNameText.color = teamStat.Key.Color;
                
                teamStatLineUi.TankLeftText.text = teamStat.Value.TankLeft.ToString();
                teamStatLineUi.LossCountText.text = teamStat.Value.LossCount.ToString();
                teamStatLineUi.DamageDoneText.text = teamStat.Value.DamageDone.ToString();
                teamStatLineUi.DamageSufferedText.text = teamStat.Value.DamageSuffered.ToString();

                teamStatLineUi.TeamKillText.text = teamStat.Value.TeamKill.ToString();
                teamStatLineUi.KillCountText.text = teamStat.Value.KillCount.ToString();
                teamStatLineUi.BonusCountText.text = teamStat.Value.BonusCount.ToString();
                teamStatLineUi.VictoryPointsText.text = teamStat.Value.VictoryPoints.ToString();
                teamStatLineUi.TotalPoints.text = teamStat.Value.TotalPoints.ToString();
            }
        }

        private void OnDisable() {
            foreach (Transform child in StatsContent.transform) {
                Destroy(child.gameObject);
            }
        }

        public void RestartMatch() {
            CurrentMatchReference.Value.ResetStats();
            OnReloadScene.Raise(GlobalProperties.Scenes.Game);
        }
        
    }
}
