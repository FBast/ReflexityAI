using UnityEngine;

namespace Examples.TankArena.Scripts.Components {
    public class Spinning : MonoBehaviour {

        public Vector3 Speed;
        
        private void Update() {
            transform.Rotate(Speed * Time.deltaTime);
        }
        
    }
}
