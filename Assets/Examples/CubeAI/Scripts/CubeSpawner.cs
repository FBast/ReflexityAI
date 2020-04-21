using System.Collections.Generic;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Examples.CubeAI.Scripts {
    public class CubeSpawner : MonoBehaviour {

        [Header("AI Parameters")]
        public List<AIBrainGraph> UtilityAIBrains;
        public float TimeBetweenRefresh;
        public ResolutionType ResolutionType;
        
        [Header("Spawn Parameters")]
        public GameObject CubePrefab;
        public int XNumber;
        public int ZNumber;
        public float Gap;

        private void Start() {
            Vector3 startingPosition = new Vector3((float) (-XNumber + 1) / 2 * Gap, 0, (float) (-ZNumber + 1) / 2 * Gap);
            Vector3 newPosition = startingPosition;
            for (int i = 0; i < XNumber; i++) {
                for (int j = 0; j < ZNumber; j++) {
                    CubeEntity cubeEntity = Instantiate(CubePrefab, newPosition, Quaternion.identity).GetComponent<CubeEntity>();
                    CubeAIComponent cubeAiComponent = cubeEntity.GetComponent<CubeAIComponent>();
                    cubeAiComponent.UtilityAiBrains = UtilityAIBrains;
                    cubeAiComponent.TimeBetweenRefresh = TimeBetweenRefresh;
                    cubeAiComponent.OptionsResolution = ResolutionType;
                    GameManager.CubeEntities.Add(cubeEntity);
                    newPosition.z += Gap;
                }
                newPosition.z = startingPosition.z;
                newPosition.x += Gap;
            }
        }
        
    }
}