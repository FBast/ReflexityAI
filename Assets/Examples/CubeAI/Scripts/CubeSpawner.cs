using UnityEngine;

namespace Examples.CubeAI.Scripts {
    public class CubeSpawner : MonoBehaviour {

        [Header("Spawn Parameters")]
        public GameObject CubePrefab;
        public int XNumber;
        public int ZNumber;
        public float Gap;

        private void Start() {
            Vector3 startingPosition = new Vector3((float) (-XNumber + 1) / 2 * Gap, 0, (float) (-ZNumber + 1) / 2 * Gap);
            Vector3 newPosition = startingPosition;
            int cubeNumber = 1;
            for (int i = 0; i < XNumber; i++) {
                for (int j = 0; j < ZNumber; j++) {
                    CubeEntity cubeEntity = Instantiate(CubePrefab, newPosition, Quaternion.identity).GetComponent<CubeEntity>();
                    cubeEntity.name = "Cube " + cubeNumber;
                    cubeNumber++;
                    newPosition.z += Gap;
                }
                newPosition.z = startingPosition.z;
                newPosition.x += Gap;
            }
        }
        
    }
}