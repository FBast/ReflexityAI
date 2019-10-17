using System;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.CubeAI {
    public class CubeEntity : MonoBehaviour {

        public MeshRenderer MeshRenderer;
        public Text Stats;
        public bool IsDead; 
        
        [Header("Parameters")]
        public int MaxHp;
        public int CurrentHp;
        public Transform CanonOutTransform;
        public GameObject ProjectilePrefab;
        public int ProjectilePower;
        public int MaxAmmo;
        public int CurrentAmmo;
        public int TargetSpeed;
        public GameObject Target;

        private Color _startingColor;
        
        private void Start() {
            _startingColor = MeshRenderer.material.color;
        }

        private void Update() {
            if (IsDead) return;
            if (Target) {
                Vector3 newDir = Vector3.RotateTowards(transform.forward, Target.transform.position - transform.position, TargetSpeed * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
            Stats.text = "HP : " + CurrentHp + " Ammo : " + CurrentAmmo;
        }

        public void Fire() {
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
            if (CurrentHp <= 0) {
                IsDead = true;
                Stats.text = "DEAD !";
                GetComponent<CubeAIComponent>().enabled = false;
            }
        }

        private void OnTriggerExit(Collider other) {
            RollBackColor();
            Destroy(other.gameObject);
        }

    }
}