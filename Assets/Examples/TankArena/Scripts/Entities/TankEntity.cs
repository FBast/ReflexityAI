using System;
using System.Collections.Generic;
using System.Linq;
using Examples.TankArena.Scripts.AI;
using Examples.TankArena.Scripts.Components;
using Examples.TankArena.Scripts.Data;
using Examples.TankArena.Scripts.Extensions;
using Examples.TankArena.Scripts.Framework;
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

        [Header("Prefabs")]
        public GameObject ShellPrefab;
        public GameObject CanonShotPrefab;
        public GameObject TankExplosionPrefab;
        public GameObject BustedTankPrefab;

        [Header	("Parameters")]
        public LayerMask CoverLayer;
        
        [Header("Debug Only")]
        public TankEntity Target;
        public Vector3 Destination;
        public int MaxHp;
        public int CurrentHp;
        
        public static List<TankEntity> TankEntities = new List<TankEntity>();
        
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
        
        public Team Team { get; private set; }
        
        public TankEntity TankInSight {
            get {
                if (!Physics.Raycast(CanonOut.position, CanonOut.forward, out var hit, Mathf.Infinity)) return null;
                return hit.transform.GetComponent<TankEntity>();
            }
        }
        
        public Transform Transform => transform;
        public bool IsShellLoaded => _timeSinceLastShot >= _reloadTime;
        public bool IsDead => CurrentHp <= 0;
        public List<TankEntity> Aggressors => TankEntities
            .Where(go => go != null && go.GetComponent<TankEntity>().Target == this).ToList();
        public int AgressorsCount => Aggressors.Count;
        private int TotalDamages => MaxHp - CurrentHp;
        private float DamagePercent => (float) TotalDamages / MaxHp;
        private bool IsAtDestination => _navMeshAgent.remainingDistance < Mathf.Infinity &&
                                         _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete &&
                                         _navMeshAgent.remainingDistance <= 0;
        
        private void Awake() {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _tankAi = GetComponent<TankAI>();
            TankEntities.Add(this);
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

        private void OnDestroy() {
            TankEntities.Remove(this);
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
            if (Destination != Vector3.zero) {
                Vector3[] pathCorners = _navMeshAgent.path.corners;
                if (pathCorners.Length > 0) {
                    Debug.DrawLine(transform.position, pathCorners[0], Color.green);
                    for (int i = 1; i < pathCorners.Length - 1; i++) {
                        Debug.DrawLine(pathCorners[i - 1], pathCorners[i], Color.green);
                    }
                }
                _navMeshAgent.SetDestination(Destination);
                if (IsAtDestination) Destination = Vector3.zero;
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
            instantiate.GetComponent<ShellEntity>().CanonDamage = _canonDamage;
            _timeSinceLastShot = 0;
        }
        
        public void DamageByShot(ShellEntity shell) {
            CurrentHp -= shell.CanonDamage;
            GlobalFields.CurrentMatch.TeamStats[Team].DamageSuffered += shell.CanonDamage;
            if (shell.TankEntityOwner.Team != Team)
                GlobalFields.CurrentMatch.TeamStats[shell.TankEntityOwner.Team].DamageDone += shell.CanonDamage;
            if (CurrentHp > 0) return;
            Die(shell.TankEntityOwner);
        }

        public void DamageByExplosion(TankEntity tank) {
            CurrentHp -= tank._explosionDamage;
            GlobalFields.CurrentMatch.TeamStats[Team].DamageSuffered += tank._explosionDamage;
            if (tank.Team != Team)
                GlobalFields.CurrentMatch.TeamStats[tank.Team].DamageDone += tank._explosionDamage;
            if (CurrentHp > 0) return;
            Die(tank, true);
        }

        private void Die(TankEntity killer, bool noExplosion = false) {
            if (killer.Team == Team)
                GlobalFields.CurrentMatch.TeamStats[Team].TeamKill++;
            else
                GlobalFields.CurrentMatch.TeamStats[killer.Team].KillCount++;
            GlobalFields.CurrentMatch.TeamStats[Team].LossCount++;
            GlobalFields.CurrentMatch.TeamStats[Team].TankLeft--;
            if (GlobalFields.CurrentMatch.TeamInMatch.Count() == 1)
                GlobalActions.OnMatchFinished.Invoke();
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
            // Disable object
            gameObject.SetActive(false);
        }

        public void Heal(int healing) {
            CurrentHp += healing;
            if (CurrentHp > MaxHp) CurrentHp = MaxHp;
        }

        public List<WaypointEntity> SeekWaypointInRadius() {
            return WaypointEntity.WaypointEntities
                .Where(go => Vector3.Distance(transform.position, go.transform.position) < _waypointRadius).ToList();
        }

        public FactionType GetFaction(TankEntity otherTank) {
            return Team == otherTank.Team ? FactionType.Ally : FactionType.Enemy;
        }

    }
}
