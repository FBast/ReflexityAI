using Examples.TankArena.Scripts.Framework;
using UnityEngine;

namespace Examples.TankArena.Scripts.UI {
    public class TankNumberUI : MonoBehaviour {

        [Header("Prefabs")] 
        public GameObject TeamTankPrefab;

        private void Update() {
            ClearTeamTanks();
            foreach (Team team in GlobalFields.CurrentMatch.Teams) {
                GameObject instantiate = Instantiate(TeamTankPrefab, transform);
                TeamTankUI teamTankUi = instantiate.GetComponent<TeamTankUI>();
                teamTankUi.TeamNameText.text = team.TeamName;
                teamTankUi.TeamNameText.color = team.Color;
                for (int i = 0; i < GlobalFields.CurrentMatch.TeamStats[team].TankLeft; i++) {
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
