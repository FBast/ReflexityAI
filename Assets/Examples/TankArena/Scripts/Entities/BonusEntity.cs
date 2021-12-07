using System.Collections.Generic;
using Examples.TankArena.Scripts.SOReferences.MatchReference;
using UnityEngine;

namespace Examples.TankArena.Scripts.Entities {
    public class BonusEntity : MonoBehaviour {
        
        [Header("Prefabs")]
        public GameObject BonusExplosionPrefab;
        
        [Header("SO References")]
        public MatchReference MatchReference;
        
        [Header("Parameters")]
        public int Healing;

        public static List<BonusEntity> BonusEntities = new List<BonusEntity>();
        
        public Transform Transform => transform;

        private void Awake() {
            BonusEntities.Add(this);
        }

        private void OnDestroy() {
            BonusEntities.Remove(this);
        }

        private void OnTriggerEnter(Collider other) {
            Instantiate(BonusExplosionPrefab, transform.position, Quaternion.identity);
            TankEntity tankEntity = other.gameObject.GetComponent<TankEntity>();
            if (tankEntity != null) {
                MatchReference.Value.TeamStats[tankEntity.Team].BonusCount++;
                Destroy(gameObject);
            }
        }
    }
}
