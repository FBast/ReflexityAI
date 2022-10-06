using System.Collections.Generic;
using System.Linq;
using Examples.TankArena.Scripts.Framework;
using Examples.TankArena.Scripts.Managers;
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
                if (teamStat.Value.TankLeft > 0 && CurrentMatchReference.Value.TeamStats.Sum(pair => pair.Value.TankLeft) == teamStat.Value.TankLeft) 
                    teamStat.Value.VictoryNumber = 1;
                teamStat.Value.TotalPoints += teamStat.Value.TeamKill * GameManager.Instance.PointPerTeamKill;
                teamStat.Value.TotalPoints += teamStat.Value.KillCount * GameManager.Instance.PointPerKill;
                teamStat.Value.TotalPoints += teamStat.Value.BonusCount * GameManager.Instance.PointPerBonus;
                teamStat.Value.TotalPoints += teamStat.Value.VictoryNumber;
            }
            // Display stats
            foreach (KeyValuePair<Team,Stats> teamStat in CurrentMatchReference.Value.TeamStats.OrderByDescending(pair => pair.Value.TotalPoints)) {
                TeamStatLineUI teamStatLineUi = Instantiate(TeamStatLine, StatsContent.transform).GetComponent<TeamStatLineUI>();
                teamStatLineUi.TeamNameText.text = teamStat.Key.TeamName;
                teamStatLineUi.TeamNameText.color = teamStat.Key.Color;
                
                teamStatLineUi.TankLeftText.text = teamStat.Value.TankLeft.ToString();
                teamStatLineUi.TankLostTest.text = teamStat.Value.LossCount.ToString();
                teamStatLineUi.DamageDoneText.text = teamStat.Value.DamageDone.ToString();
                teamStatLineUi.DamageSufferedText.text = teamStat.Value.DamageSuffered.ToString();

                teamStatLineUi.AllyKilledText.text = teamStat.Value.TeamKill + " (x" + GameManager.Instance.PointPerTeamKill + ")";
                teamStatLineUi.EnemyKilledText.text = teamStat.Value.KillCount + " (x" + GameManager.Instance.PointPerKill + ")";
                teamStatLineUi.BonusCollectedText.text = teamStat.Value.BonusCount + " (x" + GameManager.Instance.PointPerBonus + ")";
                teamStatLineUi.VictoryNumberText.text = teamStat.Value.VictoryNumber + " (x" + GameManager.Instance.PointPerVictory + ")";
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
