using System;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.CubeAI {
    public class CubeEntity : MonoBehaviour {

        public MeshRenderer MeshRenderer;
        public Text Stats;
        public bool IsDead; 
        
        [Header("Health")]
        public int MaxHp;
        public int CurrentHp;

        [Header("Weapon")] 
        public Transform CanonOutTransform;
        public GameObject ProjectilePrefab;
        public int ProjectilePower;
        public int MaxAmmo;
        public int CurrentAmmo;

        private Color _startingColor;
        
        private void Start() {
            _startingColor = MeshRenderer.material.color;
        }

        private void Update() {
            if (IsDead)
                Stats.text = "DEAD !";
            else
                Stats.text = "HP : " + CurrentHp + " Ammo : " + CurrentAmmo;
        }

        public void FireForward() {
            if (CurrentAmmo == 0)
                throw new Exception("No more ammo, the AI should not fire !");
            GameObject instantiate = Instantiate(ProjectilePrefab, CanonOutTransform.position, Quaternion.identity);
            instantiate.GetComponent<Rigidbody>().AddForce(transform.forward * ProjectilePower, ForceMode.Impulse);
            CurrentAmmo--;
        }

        public void Reload() {
            MeshRenderer.material.color = Color.blue;
            CurrentAmmo = MaxAmmo;
            Invoke("RollBackColor", 0.1f);
        }

        public void Heal() {
            if (CurrentHp == MaxHp)
                throw new Exception("Maximum health, the AI should not heal !");
            MeshRenderer.material.color = Color.green;
            CurrentHp += 2;
            if (CurrentHp > MaxHp) 
                CurrentHp = MaxHp;
            Invoke("RollBackColor", 1f);
        }

        private void RollBackColor() {
            if (IsDead)
                MeshRenderer.material.color = Color.black;
            else
                MeshRenderer.material.color = _startingColor;
        }
        
        private void OnTriggerEnter(Collider other) {
            MeshRenderer.material.color = Color.red;
            CurrentHp--;
            if (CurrentHp <= 0)
                IsDead = true;
        }

        private void OnTriggerExit(Collider other) {
            RollBackColor();
            Destroy(other.gameObject);
        }

    }
}