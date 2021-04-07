using System.Collections.Generic;
using Plugins.ReflexityAI.Framework;
using UnityEngine;

namespace Examples.TankArena.Scripts.Data {
    [CreateAssetMenu(fileName = "NewTankSetting", menuName = "TankSetting")]
    public class TankSetting : ScriptableObject {

        public string PlayerName;
        public string TankName;
        public Color TurretColor;
        public Color HullColor;
        public Color TracksColor;
        public List<AIBrainGraph> Brains = new List<AIBrainGraph>();
        
    }
}