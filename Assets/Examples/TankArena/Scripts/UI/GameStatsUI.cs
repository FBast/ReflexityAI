using System.Collections.Generic;
using System.Linq;
using Examples.TankArena.Scripts.Framework;
using Examples.TankArena.Scripts.Managers;
using Examples.TankArena.Scripts.SOEvents.StringEvents;
using Examples.TankArena.Scripts.SOReferences.GameReference;
using Examples.TankArena.Scripts.SOReferences.MatchReference;
using TMPro;
using UnityEngine;

namespace Examples.TankArena.Scripts.UI {
    public class GameStatsUI : MonoBehaviour {
        
        [Header("Prefabs")] 
        public GameObject TeamStatLine;

        [Header("Internal References")] 
        public Transform StatsContent;
        public TextMeshProUGUI ContextText;
        
        [Header("SO References")] 
        public GameReference CurrentGameReference;
        public MatchReference CurrentMatchReference;

        [Header("SO Events")] 
        public StringEvent OnReloadScene;
        public StringEvent OnUnloadScene;
        public StringEvent OnLoadScene;

        private void OnEnable() {
            // Calculate game stats
            foreach (KeyValuePair<Team,Stats> teamStat in CurrentMatchReference.Value.TeamStats) {
                CurrentGameReference.Value.TeamStats[teamStat.Key].TankLeft += teamStat.Value.TankLeft;
                CurrentGameReference.Value.TeamStats[teamStat.Key].LossCount += teamStat.Value.LossCount;
                CurrentGameReference.Value.TeamStats[teamStat.Key].DamageDone += teamStat.Value.DamageDone;
                CurrentGameReference.Value.TeamStats[teamStat.Key].DamageSuffered += teamStat.Value.DamageSuffered;
                CurrentGameReference.Value.TeamStats[teamStat.Key].TeamKill += teamStat.Value.TeamKill;
                CurrentGameReference.Value.TeamStats[teamStat.Key].KillCount += teamStat.Value.KillCount;
                CurrentGameReference.Value.TeamStats[teamStat.Key].BonusCount += teamStat.Value.BonusCount;
                CurrentGameReference.Value.TeamStats[teamStat.Key].VictoryNumber += teamStat.Value.VictoryNumber;
                CurrentGameReference.Value.TeamStats[teamStat.Key].TotalPoints += teamStat.Value.TotalPoints;
            }
            // Display stats
            foreach (KeyValuePair<Team,Stats> teamStat in CurrentGameReference.Value.TeamStats.OrderByDescending(pair => pair.Value.TotalPoints)) {
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
            if (CurrentGameReference.Value.NextMatch() != null) {
                ContextText.text = "Next match : " + string.Join(" vs ", CurrentGameReference.Value.NextMatch().Teams.Select(i => i.TeamName).ToArray());
            }
            else {
                int maxVictoryPoint = CurrentGameReference.Value.TeamStats.Max(pair => pair.Value.TotalPoints);
                ContextText.text = "Victory for " + string.Join(" & ", CurrentGameReference.Value.TeamStats
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
            if (CurrentGameReference.Value.NextMatch() != null) {
                Match match = CurrentGameReference.Value.NextMatch();
                CurrentGameReference.Value.CurrentMatch = match;
                CurrentMatchReference.Value = match;
                OnReloadScene.Raise(GlobalProperties.Scenes.Game);
            }
            else {
                OnUnloadScene.Raise(GlobalProperties.Scenes.Game);
                OnLoadScene.Raise(GlobalProperties.Scenes.Menu);
            }
        }

    }
}