using System.Collections.Generic;
using System.Linq;
using Examples.TankArena.Scripts.Data;
using UnityEngine;

namespace Examples.TankArena.Scripts.Framework {
    public class Team {

        public Color Color = Color.black;
        public List<TankSetting> TankSettings = new List<TankSetting>();
        
        public string TeamName => TankSettings
            .Select(setting => setting.PlayerName)
            .Distinct()
            .Aggregate((i, j) => i + " & " + j);
        
    }
}