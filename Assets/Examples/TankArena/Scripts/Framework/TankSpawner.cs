using System.Collections.Generic;
using Examples.TankArena.Scripts.Data;
using Examples.TankArena.Scripts.Entities;
using Examples.TankArena.Scripts.SOReferences.GameObjectListReference;
using Examples.TankArena.Scripts.SOReferences.MatchReference;
using Examples.TankArena.Scripts.Utils;
using UnityEngine;

namespace Examples.TankArena.Scripts.Framework {
    public class TankSpawner : MonoBehaviour {
    
        [Header("Prefabs")] 
        public GameObject TankPrefab;
    
        [Header("Internal References")] 
        public List<Transform> TeamPositions;
        public Transform TankContent;
  
        [Header("SO References")] 
        public MatchReference CurrentMatchReference;

        private void Start() {
            for (int i = 0; i < CurrentMatchReference.Value.Teams.Count; i++) {
                GenerateTankTeam(CurrentMatchReference.Value.Teams[i], TeamPositions[i]);
            }
        }

        private void GenerateTankTeam(Team team, Transform centerPosition) {
            // Generate
            List<GameObject> tanks = new List<GameObject>();
            for (int i = 0; i < team.TankSettings.Count; i++) {
                GameObject instantiate = Instantiate(TankPrefab, centerPosition.position, Quaternion.identity, TankContent);
                instantiate.GetComponent<TankEntity>().Init(team.TankSettings[i], team);
                instantiate.name = team.TeamName + " Tank " + i;
                tanks.Add(instantiate);
            }
            // Setup position
            List<Vector3> skirmished = FormationUtils.Skirmished(tanks, centerPosition.transform.position, 2);
            for (int i = 0; i < tanks.Count; i++) {
                tanks[i].transform.position = centerPosition.position + skirmished[i];
            }
        }

    }
}