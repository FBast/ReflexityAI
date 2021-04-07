using Examples.TankArena.Scripts.SOReferences.GameObjectListReference;
using UnityEngine;

namespace Examples.TankArena.Scripts.Entities {
    public class BonusEntity : MonoBehaviour {

        [Header("SO References")] 
        public GameObjectListReference BonusReference;
        
        [Header("Prefabs")]
        public GameObject BonusExplosionPrefab;
        
        [Header("Parameters")]
        public int Healing;

        public Transform Transform => transform;
        
        private void OnTriggerEnter(Collider other) {
            Instantiate(BonusExplosionPrefab, transform.position, Quaternion.identity);
            if (other.gameObject.GetComponent<TankEntity>()) {
                other.gameObject.GetComponent<TankEntity>().Heal(Healing);
            }
            BonusReference.Value.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
