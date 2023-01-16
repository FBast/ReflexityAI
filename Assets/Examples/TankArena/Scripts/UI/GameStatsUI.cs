using System.Collections.Generic;
using System.Linq;
using Examples.TankArena.Scripts.Framework;
using Examples.TankArena.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace Examples.TankArena.Scripts.UI {
    public class GameStatsUI : MonoBehaviour {
        
        [Header("Prefabs")] 
        public GameObject TeamStatLine;

        [Header("Internal References")] 
        public Transform StatsContent;
        public TextMeshProUGUI ContextText;

        private void OnEnable() {
            // Calculate game stats
            foreach (KeyValuePair<Team,Stats> teamStat in GlobalFields.CurrentMatch.TeamStats) {
                GlobalFields.CurrentGame.TeamStats[teamStat.Key].TankLeft += teamStat.Value.TankLeft;
                GlobalFields.CurrentGame.TeamStats[teamStat.Key].LossCount += teamStat.Value.LossCount;
                GlobalFields.CurrentGame.TeamStats[teamStat.Key].DamageDone += teamStat.Value.DamageDone;
                GlobalFields.CurrentGame.TeamStats[teamStat.Key].DamageSuffered += teamStat.Value.DamageSuffered;
                GlobalFields.CurrentGame.TeamStats[teamStat.Key].TeamKill += teamStat.Value.TeamKill;
                GlobalFields.CurrentGame.TeamStats[teamStat.Key].KillCount += teamStat.Value.KillCount;
                GlobalFields.CurrentGame.TeamStats[teamStat.Key].BonusCount += teamStat.Value.BonusCount;
                GlobalFields.CurrentGame.TeamStats[teamStat.Key].VictoryNumber += teamStat.Value.VictoryNumber;
                GlobalFields.CurrentGame.TeamStats[teamStat.Key].TotalPoints += teamStat.Value.TotalPoints;
            }
            // Display stats
            foreach (KeyValuePair<Team,Stats> teamStat in GlobalFields.CurrentGame.TeamStats.OrderByDescending(pair => pair.Value.TotalPoints)) {
                TeamStatLineUI teamStatLineUi = Instantiate(TeamStatLine, StatsContent.transform)
                    .GetComponent<TeamStatLineUI>();
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
            // Display context
            if (GlobalFields.CurrentGame.NextMatch() != null) {
                ContextText.text = "Next match : " + string.Join(" vs ", GlobalFields.CurrentGame.NextMatch().Teams.Select(i => i.TeamName).ToArray());
            }
            else {
                int maxVictoryPoint = GlobalFields.CurrentGame.TeamStats.Max(pair => pair.Value.TotalPoints);
                ContextText.text = "Victory for " + string.Join(" & ", GlobalFields.CurrentGame.TeamStats
                    .Where(pair => pair.Value.TotalPoints == maxVictoryPoint)
                    .Select(pair => pair.Key.TeamName)
                    .ToArray());
            }
        }

        private void OnDisable() {
            foreach (Transform child in StatsContent.transform) {
                Destroy(child.gameObject);
            }
        }

        public void NextMatch() {
            if (GlobalFields.CurrentGame.NextMatch() != null) {
                Match match = GlobalFields.CurrentGame.NextMatch();
                GlobalFields.CurrentGame.CurrentMatch = match;
                GlobalFields.CurrentMatch = match;
                GlobalActions.OnReloadScene.Invoke(GlobalProperties.Scenes.Game);
            }
            else {
                GlobalActions.OnUnloadScene.Invoke(GlobalProperties.Scenes.Game);
                GlobalActions.OnLoadScene.Invoke(GlobalProperties.Scenes.Menu);
            }
        }

    }
}