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
        public Renderer TurretMeshRenderer;
        public Renderer HullMeshRenderer;
        public Renderer RightTrackMeshRender;
        public Renderer LeftTrackMeshRender;
        public Renderer FactionFlag;
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
        
        [Header("Debug Only")]
        public TankEntity Target;
        public Transform Destination;
        public int MaxHp;
        public int CurrentHp;
        
        private NavMeshAgent _navMeshAgent;
        private TankAI _tankAi;
        private float _timeSinceLastShot;
        private int _canonDamage;
        private int _canonPower;
        private int _turretSpeed;
        private int _reloadTime;
        private int _explosionDamage;
        private int _explosionRadius;
        private int _waypointRadius;
        private readonly Collider[] _hitColliders = new Collider[10];
        
        public Transform Transform => transform;
        public bool IsShellLoaded => _timeSinceLastShot >= _reloadTime;
        public bool IsDead => CurrentHp <= 0;

        public Team Team { get; private set; }
        
        public TankEntity TankInSight {
            get {
                if (!Physics.Raycast(CanonOut.position, CanonOut.forward, out var hit, Mathf.Infinity)) return null;
                return hit.transform.GetComponent<TankEntity>();
            }
        }
        
        public List<GameObject> Aggressors => TanksReference.Value
            .Where(go => go != null && go.GetComponent<TankEntity>().Target == this).ToList();
        private int TotalDamages => MaxHp - CurrentHp;
        private float DamagePercent => (float) TotalDamages / MaxHp;
        private bool IsAtDestination => _navMeshAgent.remainingDistance < Mathf.Infinity &&
                                         _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete &&
                                         _navMeshAgent.remainingDistance <= 0;
        
        private void Awake() {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _tankAi = GetComponent<TankAI>();
            _navMeshAgent.speed = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.TankSpeed, GlobalProperties.PlayerPrefsDefault.TankSpeed);
            _canonDamage = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.CanonDamage, GlobalProperties.PlayerPrefsDefault.CanonDamage);
            _canonPower = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.CanonPower, GlobalProperties.PlayerPrefsDefault.CanonPower);
            _turretSpeed = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.TurretSpeed, GlobalProperties.PlayerPrefsDefault.TurretSpeed);
            _reloadTime = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.ReloadTime, GlobalProperties.PlayerPrefsDefault.ReloadTime);
            _explosionDamage = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.ExplosionDamage, GlobalProperties.PlayerPrefsDefault.ExplosionDamage);
            _explosionRadius = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.ExplosionRadius, GlobalProperties.PlayerPrefsDefault.ExplosionRadius);
            _waypointRadius = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.WaypointSeekRadius, GlobalProperties.PlayerPrefsDefault.WaypointSeekRadius);
            MaxHp = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.HealthPoints, GlobalProperties.PlayerPrefsDefault.HealthPoints);
            CurrentHp = MaxHp;
        }

        public void Init(TankSetting setting, Team team) {
            if (!setting)
                throw new Exception("Each tank need a tank setting to be set");
            Team = team;
            TurretMeshRenderer.material.color = setting.TurretColor;
            HullMeshRenderer.material.color = setting.HullColor;
            RightTrackMeshRender.material.color = setting.TracksColor;
            LeftTrackMeshRender.material.color = setting.TracksColor;
            FactionFlag.material.color = team.Color;
            _tankAi.AIBrains = setting.Brains;
            _tankAi.Init();
            _tankAi.EnqueueAI();
        }
        
        private void OnDrawGizmos() {
            Gizmos.color = new Color(0, 1, 0, 0.2f);
            Gizmos.DrawSphere(transform.position, _waypointRadius);
            Gizmos.color = new Color(1, 0, 0, 0.2f);
            Gizmos.DrawSphere(transform.position, _explosionRadius);
        }
        
        private void Update() {
            if (IsDead) return;
            if (!IsShellLoaded) _timeSinceLastShot += Time.deltaTime;
            Debug.DrawRay(CanonOut.position, CanonOut.forward * 100, Color.red);
            if (Target) {
                Vector3 newDir = Vector3.RotateTowards(Turret.forward, Target.transform.position - Turret.position, _turretSpeed * Time.deltaTime, 0.0f);
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
                if (IsAtDestination) Destination = null;
            }
            SmokeSetter.SetEmissionPercent(DamagePercent);
            FireSetter.SetEmissionPercent(DamagePercent < 0.5 ? 0 : DamagePercent * 2);
        }
        
        public void Fire() {
            if (!IsShellLoaded) return;
            Instantiate(CanonShotPrefab, CanonOut.position, CanonShotPrefab.transform.rotation);
            ShotFiring.Play();
            GameObject instantiate = Instantiate(ShellPrefab, CanonOut.position, CanonOut.rotation);
            instantiate.GetComponent<Rigidbody>().AddForce(CanonOut.transform.forward * _canonPower, ForceMode.Impulse);
            instantiate.GetComponent<ShellEntity>().TankEntityOwner = this;
            instantiate.GetComponent<ShellEntity>().Damage = _canonDamage;
            _timeSinceLastShot = 0;
        }

        public void DamageByShot(ShellEntity shell) {
            CurrentHp -= shell.Damage;
            CurrentMatchReference.Value.TeamStats[Team].DamageSuffered += shell.Damage;
            if (shell.TankEntityOwner.Team == Team)
                CurrentMatchReference.Value.TeamStats[Team].TeamDamage += shell.Damage;
            else
                CurrentMatchReference.Value.TeamStats[shell.TankEntityOwner.Team].DamageDone += shell.Damage;
            if (CurrentHp > 0) return;
            Die(shell.TankEntityOwner);
        }

        public void DamageByExplosion(TankEntity tank) {
            CurrentHp -= tank._explosionDamage;
            CurrentMatchReference.Value.TeamStats[Team].DamageSuffered += tank._explosionDamage;
            if (tank.Team == Team)
                CurrentMatchReference.Value.TeamStats[Team].TeamDamage += tank._explosionDamage;
            else
                CurrentMatchReference.Value.TeamStats[tank.Team].DamageDone += tank._explosionDamage;
            if (CurrentHp > 0) return;
            Die(tank);
        }

        private void Die(TankEntity killer, bool noExplosion = false) {
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
            if (!noExplosion) {
                Instantiate(TankExplosionPrefab, transform.position, transform.rotation);
                int size = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, _hitColliders);
                for (int i = 0; i < size; i++) {
                    TankEntity tankEntity = _hitColliders[i].GetComponent<TankEntity>();
                    if (tankEntity && tankEntity != this) tankEntity.DamageByExplosion(this);
                }
            }
            // Wreck
            if (PlayerPrefsUtils.GetBool(GlobalProperties.PlayerPrefs.ExplosionCreateBustedTank, 
                GlobalProperties.PlayerPrefsDefault.ExplosionCreateBustedTank))
                Instantiate(BustedTankPrefab, transform.position, transform.rotation);
            // Remove from list
            TanksReference.Value.Remove(gameObject);
            // Disable object
            gameObject.SetActive(false);
        }

        public void Heal(int healing) {
            CurrentHp += healing;
            if (CurrentHp > MaxHp) CurrentHp = MaxHp;
        }

        public List<GameObject> SeekWaypointInRadius() {
            return WaypointsReference.Value
                .Where(go => Vector3.Distance(transform.position, go.transform.position) < _waypointRadius).ToList();
        }

        public FactionType GetFaction(TankEntity otherTank) {
            return Team == otherTank.Team ? FactionType.Ally : FactionType.Enemy;
        }

    }
}
