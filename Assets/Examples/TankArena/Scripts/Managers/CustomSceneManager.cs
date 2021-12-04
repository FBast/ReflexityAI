using System.Collections;
using System.Collections.Generic;
using Examples.TankArena.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Examples.TankArena.Scripts.Managers {
    public class CustomSceneManager : Singleton<CustomSceneManager> {
        
        private Dictionary<string, object> _parameters;

        public void UnloadScene(string scene) {
            if (!SceneManager.GetSceneByName(scene).isLoaded) return;
            StartCoroutine(UnloadSceneAsync(scene));
        }
		
        public void LoadScene(string scene) {
            if (SceneManager.GetSceneByName(scene).isLoaded) return;
            StartCoroutine(LoadSceneAsync(scene));
        }

        public void ReloadScene(string scene) {
            StartCoroutine(ReloadSceneAsync(scene));
        }

        private IEnumerator ReloadSceneAsync(string scene) {
            yield return UnloadSceneAsync(scene);
            yield return LoadSceneAsync(scene);
        }
		
        private IEnumerator UnloadSceneAsync(string scene) {
            AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(scene);
            //Wait until the last operation fully loads to return anything
            while (!asyncLoad.isDone) {
                yield return null;
            }
        }

        private IEnumerator LoadSceneAsync(string scene) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            //Wait until the last operation fully loads to return anything
            while (!asyncLoad.isDone) {
                yield return null;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
        }
		
        public object GetParam(string paramKey) {
            if (_parameters == null) return null;
            return _parameters.ContainsKey(paramKey) ? _parameters[paramKey] : null;
        }
 
        public void SetParam(string paramKey, object paramValue) {
            if (_parameters == null)
                _parameters = new Dictionary<string, object>();
            RemoveParam(paramKey);
            _parameters.Add(paramKey, paramValue);
        }

        public void RemoveParam(string paramKey) {
            if (_parameters.ContainsKey(paramKey))
                _parameters.Remove(paramKey);
        }
		
        public void ExitGame() {
            Application.Quit();
        }
	
    }
}