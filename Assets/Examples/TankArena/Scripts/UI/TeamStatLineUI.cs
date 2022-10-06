using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Examples.TankArena.Scripts.UI {
    public class TeamStatLineUI : MonoBehaviour {

        public TextMeshProUGUI TeamNameText;
        
        public TextMeshProUGUI TankLeftText;
        [FormerlySerializedAs("LossCountText")] public TextMeshProUGUI TankLostTest;
        public TextMeshProUGUI DamageDoneText;
        public TextMeshProUGUI DamageSufferedText;

        [FormerlySerializedAs("TeamKillText")] public TextMeshProUGUI AllyKilledText;
        [FormerlySerializedAs("KillCountText")] public TextMeshProUGUI EnemyKilledText;
        [FormerlySerializedAs("BonusCountText")] public TextMeshProUGUI BonusCollectedText;
        [FormerlySerializedAs("VictoryPointsText")] public TextMeshProUGUI VictoryNumberText;
        public TextMeshProUGUI TotalPoints;

    }
}
