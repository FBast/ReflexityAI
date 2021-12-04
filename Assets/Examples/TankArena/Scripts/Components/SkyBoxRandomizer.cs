using System.Collections.Generic;
using UnityEngine;

namespace Examples.TankArena.Scripts.Components {
    public class SkyBoxRandomizer : MonoBehaviour {
        
        public List<Material> SkyBoxes;

        private void Start() {
            if (SkyBoxes.Count > 0) RenderSettings.skybox = SkyBoxes[Random.Range(0, SkyBoxes.Count)];
        }

    }
}
