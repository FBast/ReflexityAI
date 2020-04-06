using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Examples.CubeAI.Scripts {
    public class GameManager : MonoBehaviour {

        public static List<CubeEntity> CubeEntities = new List<CubeEntity>();

        private void Start() {
            CubeEntities = FindObjectsOfType<CubeEntity>().ToList();
        }

    }
}