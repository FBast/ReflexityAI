using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Examples.TankArena.Scripts.UI {
    public class TeamStatLineUI : MonoBehaviour {

        public TextMeshProUGUI TeamNameText;
        
        public TextMeshProUGUI TankLeftText;
        public TextMeshProUGUI LossCountText;
        public TextMeshProUGUI DamageDoneText;
        public TextMeshProUGUI DamageSufferedText;

        public TextMeshProUGUI TeamKillText;
        public TextMeshProUGUI KillCountText;
        public TextMeshProUGUI BonusCountText;
        public TextMeshProUGUI VictoryPointsText;
        public TextMeshProUGUI TotalPoints;

    }
}
