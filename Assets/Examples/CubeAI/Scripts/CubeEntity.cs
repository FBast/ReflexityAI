using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.CubeAI.Scripts {
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
        public int CanonSpeed;
        public CubeEntity Target;

        public static List<CubeEntity> CubeEntities = new List<CubeEntity>();
        
        public bool IsFullAmmo => CurrentAmmo >= MaxAmmo;
        public bool IsEmptyAmmo => CurrentAmmo == 0;
        public bool IsFullLife => CurrentHp >= MaxHp;
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;

        private Color _startingColor;

        private void Awake() {
            CubeEntities.Add(this);
        }

        private void Start() {
            _startingColor = MeshRenderer.material.color;
        }

        private void Update() {
            if (IsDead) return;
            if (Target) {
                Vector3 newDir = Vector3.RotateTowards(transform.forward, Target.transform.position - transform.position, CanonSpeed * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
            if (Stats)
                Stats.text = "HP : " + CurrentHp + " Ammo : " + CurrentAmmo;
        }

        public void Fire() {
            if (CurrentAmmo == 0) return;
            GameObject instantiate = Instantiate(ProjectilePrefab, CanonOutTransform.position, Quaternion.identity);
            instantiate.GetComponent<Rigidbody>().AddForce(transform.forward * ProjectilePower, ForceMode.Impulse);
            CurrentAmmo--;
        }

        public void Reload() {
            MeshRenderer.material.color = Color.blue;
            CurrentAmmo = MaxAmmo;
            Invoke(nameof(RollBackColor), 0.1f);
        }

        public void Heal() {
            MeshRenderer.material.color = Color.green;
            CurrentHp += 2;
            if (CurrentHp > MaxHp)
                CurrentHp = MaxHp;
            Invoke(nameof(RollBackColor), 1f);
        }

        private void RollBackColor() {
            MeshRenderer.material.color = IsDead ? Color.black : _startingColor;
        }
        
        private void OnTriggerEnter(Collider other) {
            if (IsDead) return;
            MeshRenderer.material.color = Color.red;
            CurrentHp--;
            if (CurrentHp <= 0) {
                IsDead = true;
                if (Stats) Stats.text = "DEAD !";
                GetComponent<CubeAI>().enabled = false;
            }
        }

        private void OnTriggerExit(Collider other) {
            RollBackColor();
            if (IsDead) return;
            Destroy(other.gameObject);
        }

        private void OnDestroy() {
            CubeEntities.Remove(this);
        }
    }
}