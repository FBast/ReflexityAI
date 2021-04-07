using System;
using System.Collections.Generic;
using System.Linq;
using Examples.TankArena.Scripts.AI;
using Examples.TankArena.Scripts.Components;
using Examples.TankArena.Scripts.Data;
using Examples.TankArena.Scripts.Extensions;
using Examples.TankArena.Scripts.Framework;
using Examples.TankArena.Scripts.SOEvents.VoidEvents;
using Examples.TankArena.Scripts.SOReferences.GameObjectListReference;
using Examples.TankArena.Scripts.SOReferences.MatchReference;
using UnityEngine;
using UnityEngine.AI;

namespace Examples.TankArena.Scripts.Entities {
    public class TankEntity : MonoBehaviour {
        
        [Header("Internal References")] 
        public Transform CanonOut;
        public Transform Turret;
        public AudioSource ShotFiring;
        public ParticleEmissionSetter SmokeSetter;
        public ParticleEmissionSetter FireSetter;
        public MeshRenderer TurretMeshRenderer;
        public MeshRenderer HullMeshRenderer;
        public MeshRenderer RightTrackMeshRender;
        public MeshRenderer LeftTrackMeshRender;
        public MeshRenderer FactionDisk;
        public GameObject TurretCamera;

        [Header("SO References")] 
        public MatchReference CurrentMatchReference;
        public GameObjectListReference WaypointsReference;
        public GameObjectListReference TanksReference;
        public GameObjectListReference BonusReference;
        
        [Header("SO Events")] 
        public VoidEvent OnMatchFinished;
        
        [Header("Prefabs")]
        public GameObject ShellPrefab;
        public GameObject CanonShotPrefab;
        public GameObject TankExplosionPrefab;
        public GameObject BustedTankPrefab;

        [Header	("Parameters")]
        public LayerMask CoverLayer;

        public Transform Transform => transform;
        public int CanonDamage { get; private set; }
        public int CanonPower { get; private set; }
        public int TurretSpeed { get; private set; }
        public int MaxHP { get; private set; }
        public int ReloadTime { get; private set; }
        public int CurrentHP { get; private set; }
        public int ExplosionDamage { get; private set; }
        public int ExplosionRadius { get; private set; }
        public int WaypointRadius { get; private set; }
        public bool IsShellLoaded = true;
        public Team Team { get; private set; }
        public TankEntity Target;
        public Transform Destination;

        private NavMeshAgent _navMeshAgent;
        private TankAI _tankAi;

        public List<GameObject> Aggressors => TanksReference.Value
            .Where(go => go != null && go.GetComponent<TankEntity>().Target == this).ToList();
        
        private readonly Collider[] _hitColliders = new Collider[10];
        private int _totalDamages => MaxHP - CurrentHP;
        private float _damagePercent => (float) _totalDamages / MaxHP;
        private bool _isAtDestination => _navMeshAgent.remainingDistance < Mathf.Infinity &&
                                          _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete &&
                                          _navMeshAgent.remainingDistance <= 0;

        private void Awake() {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _tankAi = GetComponent<TankAI>();
            MaxHP = PlayerPrefs.GetInt(Properties.PlayerPrefs.HealthPoints, Properties.PlayerPrefsDefault.HealthPoints);
            CurrentHP = MaxHP;
            CanonDamage = PlayerPrefs.GetInt(Properties.PlayerPrefs.CanonDamage, Properties.PlayerPrefsDefault.CanonDamage);
            CanonPower = PlayerPrefs.GetInt(Properties.PlayerPrefs.CanonPower, Properties.PlayerPrefsDefault.CanonPower);
            TurretSpeed = PlayerPrefs.GetInt(Properties.PlayerPrefs.TurretSpeed, Properties.PlayerPrefsDefault.TurretSpeed);
            ReloadTime = PlayerPrefs.GetInt(Properties.PlayerPrefs.ReloadTime, Properties.PlayerPrefsDefault.ReloadTime);
            ExplosionDamage = PlayerPrefs.GetInt(Properties.PlayerPrefs.ExplosionDamage, Properties.PlayerPrefsDefault.ExplosionDamage);
            ExplosionRadius = PlayerPrefs.GetInt(Properties.PlayerPrefs.ExplosionRadius, Properties.PlayerPrefsDefault.ExplosionRadius);
            WaypointRadius = PlayerPrefs.GetInt(Properties.PlayerPrefs.WaypointSeekRadius, Properties.PlayerPrefsDefault.WaypointSeekRadius);
        }

        public void Init(TankSetting setting, Team team) {
            if (!setting)
                throw new Exception("Each tank need a tank setting to be set");
            Team = team;
            TurretMeshRenderer.material.color = setting.TurretColor;
            HullMeshRenderer.material.color = setting.HullColor;
            RightTrackMeshRender.material.color = setting.TracksColor;
            LeftTrackMeshRender.material.color = setting.TracksColor;
            FactionDisk.material.color = team.Color;
            _tankAi.AIBrains = setting.Brains;
            _tankAi.Init();
            _tankAi.EnqueueAI();
        }
        
        private void OnDrawGizmos() {
            Gizmos.color = new Color(0, 1, 0, 0.2f);
            Gizmos.DrawSphere(transform.position, WaypointRadius);
            Gizmos.color = new Color(1, 0, 0, 0.2f);
            Gizmos.DrawSphere(transform.position, ExplosionRadius);
        }
        
        private void Update() {
            
            Debug.DrawRay(CanonOut.position, CanonOut.forward * 100, Color.red);
            if (Target) {
                Vector3 newDir = Vector3.RotateTowards(Turret.forward, Target.transform.position - Turret.position, TurretSpeed * Time.deltaTime, 0.0f);
                Turret.rotation = Quaternion.LookRotation(newDir);
                Turret.eulerAngles = new Vector3(0, Turret.eulerAngles.y, 0);
            }
            if (Destination) {
                Vector3[] pathCorners = _navMeshAgent.path.corners;
                if (pathCorners.Length > 0) {
                    Debug.DrawLine(transform.position, pathCorners[0], Color.green);
                    for (int i = 1; i < pathCorners.Length - 1; i++) {
                        Debug.DrawLine(pathCorners[i - 1], pathCorners[i], Color.green);
                    }
                }
                _navMeshAgent.SetDestination(Destination.position);
                if (_isAtDestination) Destination = null;
            }
            SmokeSetter.SetEmissionPercent(_damagePercent);
            FireSetter.SetEmissionPercent(_damagePercent < 0.5 ? 0 : _damagePercent * 2);
        }
        
        public void Fire() {
            if (!IsShellLoaded) return;
            Instantiate(CanonShotPrefab, CanonOut.position, CanonShotPrefab.transform.rotation);
            ShotFiring.Play();
            GameObject instantiate = Instantiate(ShellPrefab, CanonOut.position, CanonOut.rotation);
            instantiate.GetComponent<Rigidbody>().AddForce(CanonOut.transform.forward * CanonPower, ForceMode.Impulse);
            instantiate.GetComponent<ShellEntity>().TankEntityOwner = this;
            instantiate.GetComponent<ShellEntity>().Damage = CanonDamage;
            IsShellLoaded = false;
            Invoke(nameof(Reload), ReloadTime);
        }

        public void DamageByShot(ShellEntity shell) {
            CurrentHP -= shell.Damage;
            CurrentMatchReference.Value.TeamStats[Team].DamageSuffered += shell.Damage;
            if (shell.TankEntityOwner.Team == Team)
                CurrentMatchReference.Value.TeamStats[Team].TeamDamage += shell.Damage;
            else
                CurrentMatchReference.Value.TeamStats[shell.TankEntityOwner.Team].DamageDone += shell.Damage;
            if (CurrentHP > 0) return;
            Die(shell.TankEntityOwner);
        }

        public void DamageByExplosion(TankEntity tank) {
            CurrentHP -= tank.ExplosionDamage;
            CurrentMatchReference.Value.TeamStats[Team].DamageSuffered += tank.ExplosionDamage;
            if (tank.Team == Team)
                CurrentMatchReference.Value.TeamStats[Team].TeamDamage += tank.ExplosionDamage;
            else
                CurrentMatchReference.Value.TeamStats[tank.Team].DamageDone += tank.ExplosionDamage;
            if (CurrentHP > 0) return;
            Die(tank);
        }

        private void Die(TankEntity killer) {
            if (killer.Team == Team)
                CurrentMatchReference.Value.TeamStats[Team].TeamKill++;
            else
                CurrentMatchReference.Value.TeamStats[killer.Team].KillCount++;
            CurrentMatchReference.Value.TeamStats[Team].LossCount++;
            CurrentMatchReference.Value.TeamStats[Team].TankLeft--;
            if (CurrentMatchReference.Value.TeamStats[Team].TankLeft == 0) 
                CurrentMatchReference.Value.TeamStats[Team].IsDefeated = true;
            if (CurrentMatchReference.Value.TeamInMatch.Count() == 1)
                OnMatchFinished.Raise();
            // Explosion
            Instantiate(TankExplosionPrefab, transform.position, TankExplosionPrefab.transform.rotation);
            int size = Physics.OverlapSphereNonAlloc(transform.position, ExplosionRadius, _hitColliders);
            for (int i = 0; i < size; i++) {
                TankEntity tankEntity = _hitColliders[i].GetComponent<TankEntity>();
                if (tankEntity && tankEntity != this) tankEntity.DamageByExplosion(this);
            }
            // Wreck
            if (PlayerPrefsUtils.GetBool(Properties.PlayerPrefs.ExplosionCreateBustedTank, 
                Properties.PlayerPrefsDefault.ExplosionCreateBustedTank))
                Instantiate(BustedTankPrefab, transform.position, transform.rotation);
            // Remove from list
            TanksReference.Value.Remove(gameObject);
            // Destroy
            Destroy(gameObject);
        }
        
        private void Reload() {
            IsShellLoaded = true;
        }
        
        public void Heal(int healing) {
            CurrentHP += healing;
            if (CurrentHP > MaxHP) CurrentHP = MaxHP;
        }

        public List<GameObject> SeekWaypointInRadius() {
            return WaypointsReference.Value
                .Where(go => Vector3.Distance(transform.position, go.transform.position) < WaypointRadius).ToList();
        } 
        
        public GameObject TankInRay() {
            RaycastHit hit;
            if (Physics.Raycast(CanonOut.position, CanonOut.forward, out hit, Mathf.Infinity)) {
                if (hit.transform.GetComponent<TankEntity>()) return hit.transform.gameObject;
            }
            return null;
        }

        public FactionType GetFaction(TankEntity otherTank) {
            return Team == otherTank.Team ? FactionType.Ally : FactionType.Enemy;
        }

    }
}
