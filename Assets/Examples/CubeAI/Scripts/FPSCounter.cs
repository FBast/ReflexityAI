using System.Globalization;
using UnityEngine;

namespace Examples.CubeAI.Scripts {
    public class FPSCounter : MonoBehaviour {

        private readonly Vector2 _nativeSize = new Vector2(640, 480);
        private GUIStyle _style;
        private int _qty;
        private float _currentAvgFPS;
    
        private void Start() {
            _style = new GUIStyle {fontSize = (int) (20.0f * (Screen.width / _nativeSize.x))};
        }

        private void OnGUI() {
            float fps = 1.0f / Time.smoothDeltaTime;
            GUILayout.BeginVertical();
            GUILayout.Label(((int) fps).ToString(CultureInfo.InvariantCulture) + " FPS", _style);
            GUILayout.Label(((int) AverageFPS(fps)).ToString(CultureInfo.InvariantCulture) + " AVG FPS", _style);
            GUILayout.EndVertical();
        }

        private float AverageFPS(float newFPS) {
            ++_qty;
            _currentAvgFPS += (newFPS - _currentAvgFPS)/_qty;
            return _currentAvgFPS;
        }
    
    }
}
