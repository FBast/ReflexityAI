using System;
using System.Collections.Generic;

namespace Examples.TankArena.Scripts.Framework {
    [Serializable]
    public class Game {

        public Match CurrentMatch;
        public List<Team> Teams = new List<Team>();
        public List<Match> Matches = new List<Match>();
        public Dictionary<Team, Stats> TeamStats = new Dictionary<Team, Stats>();
        
        public void SetupTeamFight() {
            // Only one FFA match
            Matches.Add(new Match(Teams));
            // Game stats
            foreach (Team team in Teams) {
                TeamStats.Add(team, new Stats());
            }
        }

        public void SetupTournament() {
            // Starting with FFA
            Matches.Add(new Match(Teams));
            // Round Robin
            List<Match> RRMatches = new List<Match>();
            foreach (Team FirstTeam in Teams) {
                foreach (Team SecondTeam in Teams) {
                    if (FirstTeam == SecondTeam) continue;
                    if (RRMatches.Exists(match => match.Teams.Contains(FirstTeam) && match.Teams.Contains(SecondTeam))) continue;
                    RRMatches.Add(new Match(new List<Team> {FirstTeam, SecondTeam}));
                }
            }
            Matches.AddRange(RRMatches);
            // Game stats
            foreach (Team team in Teams) {
                TeamStats.Add(team, new Stats());
            }
        }

        public Match NextMatch() {
            if (Matches.Count == 0)
                throw new Exception("No Matches found in Game");
            if (CurrentMatch == null) return Matches[0];
            int currentMatchIndex = Matches.IndexOf(CurrentMatch);
            if (Matches.Count <= currentMatchIndex + 1) return null;
            return Matches[currentMatchIndex + 1];
        }

    }
}
