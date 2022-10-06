using System.Collections.Generic;
using Examples.TankArena.Scripts.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Examples.TankArena.Scripts.Components {
    public class SkyBoxRandomizer : MonoBehaviour {
        
        public List<Material> SkyBoxes;

        private Material _skyBox;

        private void Awake() {
            if (SkyBoxes.Count <= 0) return; 
            _skyBox = SkyBoxes[Random.Range(0, SkyBoxes.Count)];
            RenderSettings.skybox = _skyBox;
            CustomSceneManager.Instance.OnNewActiveScene = delegate {
                RenderSettings.skybox = _skyBox;
            };
        }

    }
}