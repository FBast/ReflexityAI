using System.Collections.Generic;
using System.Linq;
using Examples.TankArena.Scripts.Entities;
using Examples.TankArena.Scripts.Framework;
using Plugins.Reflexity.Framework;
using UnityEngine;

namespace Examples.TankArena.Scripts.AI {
    public class TankAI : ReflexityAI {

        // Your custom references here
        [HideInInspector] public TankEntity TankEntity;
        public FactionType AllyFactionType = FactionType.Ally;
        public FactionType EnemyFactionType = FactionType.Enemy;
        public FactionType AllFactionType = FactionType.All;

        public IEnumerable<BonusEntity> BonusEntities => BonusEntity.BonusEntities;
        public IEnumerable<TankEntity> TankEntities => TankEntity.TankEntities;
        public IEnumerable<WaypointEntity> WaypointEntities => WaypointEntity.WaypointEntities;
        public IEnumerable<TankEntity> EnnemyTankEntities => TankEntities
            .Where(entity => entity.GetFaction(TankEntity) == FactionType.Enemy);
        public IEnumerable<TankEntity> AllyTankEntities => TankEntities
            .Where(entity => entity.GetFaction(TankEntity) == FactionType.Ally);


        // End of custom references

        private void Awake() {
            TankEntity = GetComponent<TankEntity>();
        }

    }
}