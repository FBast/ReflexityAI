using UnityEngine;

namespace Examples.TankArena.Scripts.Entities {
    public class ShellEntity : MonoBehaviour {

        [Header("Prefabs")]
        public GameObject ExplosionPrefab;

        [HideInInspector] public TankEntity TankEntityOwner;
        [HideInInspector] public int Damage;

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<ShellEntity>()) return;
            TankEntity tankEntity = other.GetComponent<TankEntity>();
            if (tankEntity == TankEntityOwner) return;
            if (tankEntity) tankEntity.DamageByShot(this);
            Instantiate(ExplosionPrefab, transform.position, Quaternion.Inverse(ExplosionPrefab.transform.rotation));
            Destroy(gameObject);
        }

    }
}
