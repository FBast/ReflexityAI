using System;
using System.Collections.Generic;
using UnityEngine;

namespace Examples.TankArena.Scripts.Components {
    public class ParticleEmissionSetter : MonoBehaviour {

        [Serializable]
        public struct ParticleEmissionSetting {
            public ParticleSystem ParticleSystem;
            public float RateOverTime;
        }

        public List<ParticleEmissionSetting> ParticleEmissionSettings;

        public void SetEmissionPercent(float percent) {
            foreach (ParticleEmissionSetting emissionSetting in ParticleEmissionSettings) {
                ParticleSystem.EmissionModule emissionModule = emissionSetting.ParticleSystem.emission;
                emissionModule.rateOverTime = percent * emissionSetting.RateOverTime;
            }
        }

    }
}
