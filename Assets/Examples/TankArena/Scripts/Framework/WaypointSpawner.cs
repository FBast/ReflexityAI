using System.Collections.Generic;
using Examples.TankArena.Scripts.Extensions;
using Examples.TankArena.Scripts.SOReferences.GameObjectListReference;
using UnityEngine;

namespace Examples.TankArena.Scripts.Framework {
    public class WaypointSpawner : MonoBehaviour {

        [Header("Prefabs")] 
        public GameObject WaypointPrefab;

        [Header("Internal References")]
        public Transform GridStart;
        public Transform GridEnd;
        public Transform WaypointContent;

        [Header("SO References")] 
        public GameObjectListReference WaypointsReference;
        
        private int _gridGap;
        
        private void Start() {
            WaypointsReference.Value = new List<GameObject>();
            _gridGap = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.GridGap, GlobalProperties.PlayerPrefsDefault.GridGap);
            GenerateWaypoints();
        }

        private void GenerateWaypoints() {

            float positionX = GridEnd.position.x - GridStart.position.x;
            float positionZ = GridEnd.position.z - GridStart.position.z;
            int xSign = positionX < 0 ? -1 : 1;
            int ySign = positionZ < 0 ? -1 : 1;
            for (int i = 0; i < Mathf.Abs(positionX) / _gridGap; i++) {
                for (int j = 0; j < Mathf.Abs(positionZ) / _gridGap; j++) {
                    Vector3 position = new Vector3(GridStart.position.x + i * _gridGap * xSign, 
                        0, GridStart.position.z + j * _gridGap * ySign);
                    if (!position.IsPositionOnNavMesh()) continue;
                    GameObject instantiate = Instantiate(WaypointPrefab, position, Quaternion.identity, WaypointContent);
                    instantiate.name = (int) position.x + ", " + (int) position.z;
                    WaypointsReference.Value.Add(instantiate);
                }
            }
        }

    }
}
