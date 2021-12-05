using System.Collections.Generic;
using Examples.TankArena.Scripts.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Examples.TankArena.Scripts.Components {
    public class SkyBoxRandomizer : MonoBehaviour {
        
        public List<Material> SkyBoxes;

        private void Awake() {
            CustomSceneManager.Instance.OnNewActiveScene = delegate {
                if (SkyBoxes.Count > 0) RenderSettings.skybox = SkyBoxes[Random.Range(0, SkyBoxes.Count)];
            };
        }

    }
}