namespace Examples.TankArena.Scripts.Framework {
    public static class Properties {

        public struct Inputs {
            public const string LeftClick = "Fire1";
            public const string RightClick = "Fire2";
            public const string WheelClick = "Fire3";
        }

        public struct Scenes {
            public const string Shared = "Shared";
            public const string Menu = "Menu";
            public const string Game = "Game";
        }

        public struct PlayerPrefs {
            public const string MatchDuration = "MatchTime";
            public const string ExplosionDamage = "ExplosionDamage";
            public const string ExplosionRadius = "ExplosionRadius";
            public const string ExplosionCreateBustedTank = "ExplosionCreateBustedTank";
            public const string CanonDamage = "CanonDamage";
            public const string CanonPower = "CanonPower";
            public const string TurretSpeed = "TurretSpeed";
            public const string HealthPoints = "MaxHp";
            public const string ReloadTime = "ReloadTime";
            public const string WaypointSeekRadius = "WaypointSeekRadius";
            public const string AlwaysPickBestChoice = "AlwaysPickBestChoice";
            public const string SecondsBetweenRefresh = "TimeBetweenRefresh";
            public const string GridGap = "GridGap";
            public const string BonusPerSpawnNumber = "BonusPerSpawnNumber";
            public const string BonusPerSpawnFrequency = "BonusPerSpawnFrequency";
        }
        
        public struct PlayerPrefsDefault {
            public const int MatchDuration = 120;
            public const int ExplosionDamage = 1;
            public const int ExplosionRadius = 5;
            public const bool ExplosionCreateBustedTank = true;
            public const int CanonDamage = 1;
            public const int CanonPower = 50;
            public const int TurretSpeed = 10;
            public const int HealthPoints = 5;
            public const int ReloadTime = 5;
            public const int WaypointSeekRadius = 50;
            public const bool AlwaysPickBestChoice = true;
            public const int SecondsBetweenRefresh = 1;
            public const int GridGap = 20;
            public const int BonusPerSpawnNumber = 3;
            public const int BonusPerSpawnFrequency = 10;
        }
        
    }
}