using System.Collections.Generic;
using System.Linq;
using Examples.TankArena.Scripts.Data;
using UnityEditor;

namespace Examples.TankArena.Scripts.Managers {
    public static class DataManager {
        
        public static List<TankSetting> TankSettings => AssetDatabase.FindAssets("t:TankSetting", new[] {"Assets"})
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<TankSetting>)
            .ToList();
        
    }
}