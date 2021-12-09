using System.Collections.Generic;
using System.Linq;
using Examples.TankArena.Scripts.Data;
using UnityEngine;

namespace Examples.TankArena.Scripts.Managers {
    public class DataManager : MonoBehaviour {

        public static List<TankSetting> TankSettings;
        
        private void Awake() {
            TankSettings = Resources.LoadAll<TankSetting>("Tanks").ToList();
        }


        
    }
}