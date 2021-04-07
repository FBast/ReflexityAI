using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.TankArena.Scripts.UI {
    public class SliderWithLabelUI : MonoBehaviour {

        [Header("Internal References")]
        public Slider Slider;
        public TextMeshProUGUI Text;

        [Header("Event")] 
        public Slider.SliderEvent OnValueChanged;

        public float Value {
            get { return Slider.value; }
            set {
                Slider.value = value;
                Text.text = value.ToString(CultureInfo.CurrentCulture);
            }
        }
        
        private void Awake() {
            Slider.onValueChanged.AddListener(delegate(float value) {
                OnValueChanged.Invoke(value);
                Text.text = value.ToString(CultureInfo.CurrentCulture);
            });
        }

    }
}