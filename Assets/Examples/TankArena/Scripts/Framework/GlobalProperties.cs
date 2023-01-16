namespace Examples.TankArena.Scripts.Framework {
    public static class GlobalProperties {
        
        public struct Scenes {
            public static string Shared => "Shared";
            public static string Menu => "Menu";
            public static string Game => "Game";
        }
        
        public struct PlayerPrefs {
            public static string MatchDuration => "MatchTime";
            public static string PointsPerKill => "PointsPerKill";
            public static string PointsPerTeamKill => "PointsPerTeamKill";
            public static string PointsPerBonus => "PointsPerBonus";
            public static string PointsForVictory => "PointsIfVictory";
            public static string ExplosionDamage => "ExplosionDamage";
            public static string ExplosionRadius => "ExplosionRadius";
            public static string ExplosionCreateBustedTank => "ExplosionCreateBustedTank";
            public static string CanonDamage => "CanonDamage";
            public static string CanonPower => "CanonPower";
            public static string TurretSpeed => "TurretSpeed";
            public static string HealthPoints => "MaxHp";
            public static string ReloadTime => "ReloadTime";
            public static string WaypointSeekRadius => "WaypointSeekRadius";
            public static string AlwaysPickBestChoice => "AlwaysPickBestChoice";
            public static string SecondsBetweenRefresh => "TimeBetweenRefresh";
            public static string GridGap => "GridGap";
            public static string BonusPerSpawnNumber => "BonusPerSpawnNumber";
            public static string BonusPerSpawnFrequency => "BonusPerSpawnFrequency";
            public static string TankSpeed => "TankSpeed";
        }
        
        public struct PlayerPrefsDefault {
            public static int MatchDuration => 60;
            public static int PointsPerKill => 2;
            public static int PointsPerTeamKill => -1;
            public static int PointsPerBonus => 1;
            public static int PointsForVictory => 3;
            public static int ExplosionDamage => 1;
            public static int ExplosionRadius => 5;
            public static bool ExplosionCreateBustedTank => true;
            public static int CanonDamage => 1;
            public static int CanonPower => 100;
            public static int TurretSpeed => 10;
            public static int HealthPoints => 5;
            public static int ReloadTime => 3;
            public static int WaypointSeekRadius => 50;
            public static bool AlwaysPickBestChoice => true;
            public static int SecondsBetweenRefresh => 1;
            public static int GridGap => 20;
            public static int BonusPerSpawnNumber => 3;
            public static int BonusPerSpawnFrequency => 10;
            public static int TankSpeed => 10;
        }

    }
}