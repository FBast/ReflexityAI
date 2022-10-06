using UnityEngine;

namespace Examples.TankArena.Scripts.Extensions {
    public static class PlayerPrefsUtils {
        
        public static void SetBool(string key, bool booleanValue) {
            PlayerPrefs.SetInt(key, booleanValue ? 1 : 0);
        }
 
        public static bool GetBool(string key) {
            return PlayerPrefs.GetInt(key) == 1;
        }
 
        public static bool GetBool(string key, bool defaultValue) {
            return PlayerPrefs.HasKey(key) ? GetBool(key) : defaultValue;
        }
        
    }
}