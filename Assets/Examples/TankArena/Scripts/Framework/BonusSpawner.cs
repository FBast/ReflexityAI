using System.Collections.Generic;
using Examples.TankArena.Scripts.SOReferences.GameObjectListReference;
using UnityEngine;

namespace Examples.TankArena.Scripts.Framework {
    public class BonusSpawner : MonoBehaviour {

        [Header("Prefabs")]
        public GameObject BonusPrefab;
        
        [Header("Parameters")]
        public string BonusName;

        [Header("SO References")] 
        public GameObjectListReference BonusReference;
        
        private GameObject _spawnedBonus;
        private float _timeSinceBonusUsed;
        private int _spawnedNumber;
        private int _spawnRate;
        private int _spawnNumber;
        
        private void Awake() {
            BonusReference.Value = new List<GameObject>();
            _spawnNumber = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.BonusPerSpawnNumber, GlobalProperties.PlayerPrefsDefault.BonusPerSpawnNumber);
            _spawnRate = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.BonusPerSpawnFrequency, GlobalProperties.PlayerPrefsDefault.BonusPerSpawnFrequency);
        }

        private void Update() {
            if (_spawnedBonus || _spawnedNumber >= _spawnNumber) return;
            _timeSinceBonusUsed += Time.deltaTime;
            if (_timeSinceBonusUsed > _spawnRate) {
                Vector3 spawnPosition = transform.position;
                spawnPosition.y = BonusPrefab.transform.position.y;
                _spawnedBonus = Instantiate(BonusPrefab, spawnPosition, Quaternion.identity, transform);
                _spawnedBonus.name = BonusName;
                BonusReference.Value.Add(_spawnedBonus);
                _timeSinceBonusUsed = 0;
                _spawnedNumber++;
            }
        }

    }
}
