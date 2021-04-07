using UnityEngine;

namespace Examples.TankArena.Scripts.Components {
    public class DestroyAfterTime : MonoBehaviour {

        public int Time = 1;
        
        private void Start() {
            Destroy(gameObject, Time);
        }
        
    }
}
