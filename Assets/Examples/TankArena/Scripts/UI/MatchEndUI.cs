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
            // Calculate victory points
            int maxTank = CurrentMatchReference.Value.TeamStats.Max(pair => pair.Value.TankLeft);
            int maxDamage = CurrentMatchReference.Value.TeamStats.Max(pair => pair.Value.DamageDone - pair.Value.TeamDamage);
            int maxKill = CurrentMatchReference.Value.TeamStats.Max(pair => pair.Value.KillCount - pair.Value.TeamKill);
            foreach (KeyValuePair<Team,Stats> teamStat in CurrentMatchReference.Value.TeamStats) {
                teamStat.Value.VictoryPoint += teamStat.Value.TankLeft == maxTank ? 1 : 0;
                teamStat.Value.VictoryPoint += teamStat.Value.DamageDone - teamStat.Value.TeamDamage == maxDamage ? 1 : 0;
                teamStat.Value.VictoryPoint += teamStat.Value.KillCount - teamStat.Value.TeamKill == maxKill ? 1 : 0;
            }
            // Display stats
            foreach (KeyValuePair<Team,Stats> teamStat in CurrentMatchReference.Value.TeamStats.OrderByDescending(pair => pair.Value.VictoryPoint)) {
                TeamStatLineUI teamStatLineUi = Instantiate(TeamStatLine, StatsContent.transform)
                    .GetComponent<TeamStatLineUI>();
                teamStatLineUi.VictoryPointText.text = teamStat.Value.VictoryPoint.ToString();
                teamStatLineUi.TeamNameText.text = teamStat.Key.TeamName;
                teamStatLineUi.TeamNameText.color = teamStat.Key.Color;
                teamStatLineUi.TankLeftText.text = teamStat.Value.TankLeft.ToString();
                teamStatLineUi.KillCountText.text = teamStat.Value.KillCount.ToString();
                teamStatLineUi.LossCountText.text = teamStat.Value.LossCount.ToString();
                teamStatLineUi.TeamKillText.text = teamStat.Value.TeamKill.ToString();
                teamStatLineUi.DamageDoneText.text = teamStat.Value.DamageDone.ToString();
                teamStatLineUi.DamageSufferedText.text = teamStat.Value.DamageSuffered.ToString();
                teamStatLineUi.TeamDamageText.text = teamStat.Value.TeamDamage.ToString();
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
