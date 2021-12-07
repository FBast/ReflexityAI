using Examples.TankArena.Scripts.Extensions;
using Examples.TankArena.Scripts.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.TankArena.Scripts.UI {
    public class SettingsUI : MonoBehaviour {

        [Header("Internal References")] 
        public SliderWithLabelUI MatchDuration;
        public SliderWithLabelUI PointsPerTeamKill;
        public SliderWithLabelUI PointsPerKill;
        public SliderWithLabelUI PointsPerBonus;
        public SliderWithLabelUI PointsForVictory;
        public SliderWithLabelUI HealthPoints;
        public SliderWithLabelUI CanonDamage;
        public SliderWithLabelUI CanonPower;
        public SliderWithLabelUI TurretSpeed;
        public SliderWithLabelUI TankSpeed;
        public SliderWithLabelUI ReloadTime;
        public SliderWithLabelUI WaypointSeekRadius;
        public SliderWithLabelUI ExplosionDamage;
        public SliderWithLabelUI ExplosionRadius;
        public Toggle ExplosionCreateBustedTank;
        public SliderWithLabelUI SecondsBetweenRefresh;
        public Toggle AlwaysPickBestChoice;
        public SliderWithLabelUI GridGap;
        public SliderWithLabelUI BonusPerSpawnNumber;
        public SliderWithLabelUI BonusPerSpawnFrequency;
        
        private void Start() {
            MatchDuration.OnValueChanged.AddListener(delegate(float value) {
                PlayerPrefs.SetInt(GlobalProperties.PlayerPrefs.MatchDuration, (int) value);
            });
            PointsPerTeamKill.OnValueChanged.AddListener(delegate(float value) {
                PlayerPrefs.SetInt(GlobalProperties.PlayerPrefs.PointsPerTeamKill, (int) value);
            });
            PointsPerKill.OnValueChanged.AddListener(delegate(float value) {
                PlayerPrefs.SetInt(GlobalProperties.PlayerPrefs.PointsPerKill, (int) value);
            });
            PointsPerBonus.OnValueChanged.AddListener(delegate(float value) {
                PlayerPrefs.SetInt(GlobalProperties.PlayerPrefs.PointsPerBonus, (int) value);
            });
            PointsForVictory.OnValueChanged.AddListener(delegate(float value) {
                PlayerPrefs.SetInt(GlobalProperties.PlayerPrefs.PointsForVictory, (int) value);
            });
            HealthPoints.OnValueChanged.AddListener(delegate(float value) {
                PlayerPrefs.SetInt(GlobalProperties.PlayerPrefs.HealthPoints, (int) value);
            });
            CanonDamage.OnValueChanged.AddListener(delegate(float value) {
                PlayerPrefs.SetInt(GlobalProperties.PlayerPrefs.CanonDamage, (int) value);
            });
            CanonPower.OnValueChanged.AddListener(delegate(float value) {
                PlayerPrefs.SetInt(GlobalProperties.PlayerPrefs.CanonPower, (int) value);
            });
            TurretSpeed.OnValueChanged.AddListener(delegate(float value) {
                PlayerPrefs.SetInt(GlobalProperties.PlayerPrefs.TurretSpeed, (int) value);
            });
            TankSpeed.OnValueChanged.AddListener(delegate(float value) {
                PlayerPrefs.SetInt(GlobalProperties.PlayerPrefs.TankSpeed, (int) value);
            });
            ReloadTime.OnValueChanged.AddListener(delegate(float value) {
                PlayerPrefs.SetInt(GlobalProperties.PlayerPrefs.ReloadTime, (int) value);
            });
            WaypointSeekRadius.OnValueChanged.AddListener(delegate(float value) {
                PlayerPrefs.SetInt(GlobalProperties.PlayerPrefs.WaypointSeekRadius, (int) value);
            });
            ExplosionDamage.OnValueChanged.AddListener(delegate(float value) {
                PlayerPrefs.SetInt(GlobalProperties.PlayerPrefs.ExplosionDamage, (int) value);
            });
            ExplosionRadius.OnValueChanged.AddListener(delegate(float value) {
                PlayerPrefs.SetInt(GlobalProperties.PlayerPrefs.ExplosionRadius, (int) value);
            });
            ExplosionCreateBustedTank.onValueChanged.AddListener(delegate(bool value) {
                PlayerPrefsUtils.SetBool(GlobalProperties.PlayerPrefs.ExplosionCreateBustedTank, value);
            });
            SecondsBetweenRefresh.OnValueChanged.AddListener(delegate(float value) {
                PlayerPrefs.SetInt(GlobalProperties.PlayerPrefs.SecondsBetweenRefresh, (int) value);
            });
            AlwaysPickBestChoice.onValueChanged.AddListener(delegate(bool value) {
                PlayerPrefsUtils.SetBool(GlobalProperties.PlayerPrefs.AlwaysPickBestChoice, value);
            });
            GridGap.OnValueChanged.AddListener(delegate(float value) {
                PlayerPrefs.SetInt(GlobalProperties.PlayerPrefs.GridGap, (int) value);
            });
            BonusPerSpawnNumber.OnValueChanged.AddListener(delegate(float value) {
                PlayerPrefs.SetInt(GlobalProperties.PlayerPrefs.BonusPerSpawnNumber, (int) value);
            });
            BonusPerSpawnFrequency.OnValueChanged.AddListener(delegate(float value) {
                PlayerPrefs.SetInt(GlobalProperties.PlayerPrefs.BonusPerSpawnFrequency, (int) value);
            });
            UpdateSettings();
        }

        private void UpdateSettings() {
            MatchDuration.Value = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.MatchDuration,
                GlobalProperties.PlayerPrefsDefault.MatchDuration);
            PointsPerTeamKill.Value = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.PointsPerTeamKill,
                GlobalProperties.PlayerPrefsDefault.PointsPerTeamKill);
            PointsPerKill.Value = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.PointsPerKill,
                GlobalProperties.PlayerPrefsDefault.PointsPerKill);
            PointsPerBonus.Value = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.PointsPerBonus,
                GlobalProperties.PlayerPrefsDefault.PointsPerBonus);
            PointsForVictory.Value = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.PointsForVictory,
                GlobalProperties.PlayerPrefsDefault.PointsForVictory);
            HealthPoints.Value = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.HealthPoints,
                GlobalProperties.PlayerPrefsDefault.HealthPoints);
            CanonDamage.Value = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.CanonDamage,
                GlobalProperties.PlayerPrefsDefault.CanonDamage);
            CanonPower.Value = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.CanonPower,
                GlobalProperties.PlayerPrefsDefault.CanonPower);
            TurretSpeed.Value = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.TurretSpeed,
                GlobalProperties.PlayerPrefsDefault.TurretSpeed);
            TankSpeed.Value = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.TankSpeed,
                GlobalProperties.PlayerPrefsDefault.TankSpeed);
            ReloadTime.Value = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.ReloadTime,
                GlobalProperties.PlayerPrefsDefault.ReloadTime);
            WaypointSeekRadius.Value = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.WaypointSeekRadius,
                GlobalProperties.PlayerPrefsDefault.WaypointSeekRadius);
            ExplosionDamage.Value = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.ExplosionDamage,
                GlobalProperties.PlayerPrefsDefault.ExplosionDamage);
            ExplosionRadius.Value = PlayerPrefs.GetInt(GlobalProperties.PlayerPrefs.ExplosionRadius,
                GlobalProperties.PlayerPrefsDefault.ExplosionRadius);
            ExplosionCreateBustedTank.isOn = PlayerPrefsUtils.GetBool(GlobalProperties.PlayerPrefs.ExplosionCreateBustedTank,
                GlobalProperties.PlayerPrefsDefault.ExplosionCreateBustedTank);
            SecondsBetweenRefresh.Value = PlayerPrefs.GetFloat(GlobalProperties.PlayerPrefs.SecondsBetweenRefresh,
                GlobalProperties.PlayerPrefsDefault.SecondsBetweenRefresh);
            AlwaysPickBestChoice.isOn = PlayerPrefsUtils.GetBool(GlobalProperties.PlayerPrefs.AlwaysPickBestChoice,
                GlobalProperties.PlayerPrefsDefault.AlwaysPickBestChoice);
            GridGap.Value = PlayerPrefs.GetFloat(GlobalProperties.PlayerPrefs.GridGap,
                GlobalProperties.PlayerPrefsDefault.GridGap);
            BonusPerSpawnNumber.Value = PlayerPrefs.GetFloat(GlobalProperties.PlayerPrefs.BonusPerSpawnNumber,
                GlobalProperties.PlayerPrefsDefault.BonusPerSpawnNumber);
            BonusPerSpawnFrequency.Value = PlayerPrefs.GetFloat(GlobalProperties.PlayerPrefs.BonusPerSpawnFrequency,
                GlobalProperties.PlayerPrefsDefault.BonusPerSpawnFrequency);
        }
        
        public void ResetDefault() {
            PlayerPrefs.DeleteAll();
            UpdateSettings();
        }
        
    }
}