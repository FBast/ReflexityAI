using Examples.TankArena.Scripts.Framework;
using Examples.TankArena.Scripts.SOReferences.MatchReference;
using UnityEngine;

namespace Examples.TankArena.Scripts.UI {
    public class TankNumberUI : MonoBehaviour {

        [Header("Prefabs")] 
        public GameObject TeamTankPrefab;

        [Header("SO References")] 
        public MatchReference CurrentMatchReference;

        private void Update() {
            ClearTeamTanks();
            foreach (Team team in CurrentMatchReference.Value.Teams) {
                GameObject instantiate = Instantiate(TeamTankPrefab, transform);
                TeamTankUI teamTankUi = instantiate.GetComponent<TeamTankUI>();
                teamTankUi.TeamNameText.text = team.TeamName;
                teamTankUi.TeamNameText.color = team.Color;
                for (int i = 0; i < CurrentMatchReference.Value.TeamStats[team].TankLeft; i++) {
                    teamTankUi.AddTankImage();
                }
            }
        }

        private void ClearTeamTanks() {
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }
        }
        
    }
}
