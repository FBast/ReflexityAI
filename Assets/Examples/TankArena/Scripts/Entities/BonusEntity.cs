using System.Collections.Generic;
using UnityEngine;

namespace Examples.TankArena.Scripts.Entities {
    public class BonusEntity : MonoBehaviour {

        [Header("Prefabs")]
        public GameObject BonusExplosionPrefab;
        
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
            if (other.gameObject.GetComponent<TankEntity>()) {
                other.gameObject.GetComponent<TankEntity>().Heal(Healing);
            }
            Destroy(gameObject);
        }
    }
}
