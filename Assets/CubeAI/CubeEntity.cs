using System;
using UnityEngine;
using UnityEngine.UI;

namespace CubeAI {
    public class CubeEntity : MonoBehaviour {

        public MeshRenderer MeshRenderer;
        public Text Stats;
        
        [Header("Health")]
        public int MaxHp = 10;
        public int CurrentHp;

        [Header("Weapon")] 
        public Transform CanonOutTransform;
        public GameObject ProjectilePrefab;
        public int ProjectilePower;
        public int MaxAmmo = 30;
        public int CurrentAmmo;

        private Color _startingColor;
        
        private void Start() {
            _startingColor = MeshRenderer.material.color;
        }

        private void Update() {
            Stats.text = "HP : " + CurrentHp + " Ammo : " + CurrentAmmo;
        }

        public void FireForward() {
            if (CurrentAmmo == 0)
                throw new Exception("No more ammo, impossible to Fire");
            GameObject instantiate = Instantiate(ProjectilePrefab, CanonOutTransform.position, Quaternion.identity);
            instantiate.GetComponent<Rigidbody>().AddForce(transform.forward * ProjectilePower, ForceMode.Impulse);
            CurrentAmmo--;
        }

        public void Reload() {
            MeshRenderer.material.color = Color.blue;
            CurrentAmmo = MaxAmmo;
            Invoke("RollBackColor", 1f);
        }

        public void Heal() {
            MeshRenderer.material.color = Color.green;
            CurrentHp = MaxHp;
            Invoke("RollBackColor", 1f);
        }

        private void RollBackColor() {
            MeshRenderer.material.color = _startingColor;
        }
        
        private void OnTriggerEnter(Collider other) {
            MeshRenderer.material.color = Color.red;
            CurrentHp--;
            if (CurrentHp <= 0)
                MeshRenderer.material.color = Color.black;
        }

        private void OnTriggerExit(Collider other) {
            RollBackColor();
            Destroy(other.gameObject);
        }

    }
}