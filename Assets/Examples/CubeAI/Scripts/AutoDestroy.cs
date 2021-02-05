using UnityEngine;

namespace Examples.CubeAI.Scripts {
    public class AutoDestroy : MonoBehaviour {

        public float Time;
        
        private void Awake() {
            Invoke(nameof(DestroySelf), Time);
        }

        private void DestroySelf() {
            Destroy(gameObject);
        }
        
    }
}
