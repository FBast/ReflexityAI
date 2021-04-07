using System;
using System.Collections.Generic;
using System.Linq;

namespace Examples.TankArena.Scripts.Framework {
    [Serializable]
    public class Match {

        public List<Team> Teams;
        public Dictionary<Team, Stats> TeamStats = new Dictionary<Team, Stats>();
 
        public IEnumerable<Team> TeamInMatch => TeamStats
            .Where(pair => !pair.Value.IsDefeated)
            .Select(pair => pair.Key);
        
        public Match(List<Team> teams) {
            Teams = teams;
            foreach (Team team in teams) {
                Stats stats = new Stats {TankLeft = team.TankSettings.Count};
                TeamStats.Add(team, stats);
            }
        }

        public void ResetStats() {
            TeamStats.Clear();
            foreach (Team team in Teams) {
                Stats stats = new Stats {TankLeft = team.TankSettings.Count};
                TeamStats.Add(team, stats);
            }
        }
        
    }
}
