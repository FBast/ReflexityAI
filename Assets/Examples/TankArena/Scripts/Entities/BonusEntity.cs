using System.Collections.Generic;
using Examples.TankArena.Scripts.Framework;
using UnityEngine;

namespace Examples.TankArena.Scripts.Entities {
    public class BonusEntity : MonoBehaviour {
        
        [Header("Prefabs")]
        public GameObject BonusExplosionPrefab;

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
                GlobalFields.CurrentMatch.TeamStats[tankEntity.Team].BonusCount++;
                Destroy(gameObject);
            }
        }
    }
}
