using System.Globalization;
using Examples.TankArena.Scripts.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.TankArena.Scripts.UI {
    public class TimeChronoUI : MonoBehaviour {

        [Header("Internal References")]
        public Image TimeCircle;
        public TextMeshProUGUI TimeText;

        private int _timeLeft;
    
        private void Update() {
            _timeLeft = Mathf.FloorToInt(GlobalFields.MaxTime - GlobalFields.CurrentTime);
            TimeText.text = _timeLeft.ToString(CultureInfo.CurrentCulture);
            TimeCircle.fillAmount = 1 - GlobalFields.CurrentTime / GlobalFields.MaxTime;
        }
    }
}
