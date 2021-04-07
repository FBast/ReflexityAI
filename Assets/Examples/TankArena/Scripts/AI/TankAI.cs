using System.Linq;
using Examples.TankArena.Scripts.Entities;
using Examples.TankArena.Scripts.Framework;
using Plugins.ReflexityAI.Framework;
using UnityEngine;

namespace Examples.TankArena.Scripts.AI {
    public class TankAI : ReflexityAI {

        // Your custom references here
        [HideInInspector] public TankEntity TankEntity;
        public FactionType AllyFactionType = FactionType.Ally;
        public FactionType EnemyFactionType = FactionType.Enemy;
        public FactionType AllFactionType = FactionType.All;
        public BonusEntity[] BonusEntities => GetComponent<TankEntity>().BonusReference.Value
            .Select(o => o.GetComponent<BonusEntity>())
            .ToArray();
        public TankEntity[] TankEntities => GetComponent<TankEntity>().TanksReference.Value
            .Select(o => o.GetComponent<TankEntity>())
            .ToArray();
        public TankEntity[] EnnemyTankEntities => TankEntities
            .Where(entity => entity.GetFaction(TankEntity) == FactionType.Enemy)
            .ToArray();
        public TankEntity[] AllyTankEntities => TankEntities
            .Where(entity => entity.GetFaction(TankEntity) == FactionType.Ally)
            .ToArray();
        public WaypointEntity[] WaypointEntities => GetComponent<TankEntity>().WaypointsReference.Value
            .Select(o => o.GetComponent<WaypointEntity>())
            .ToArray();
        // End of custom references

        private void Awake() {
            TankEntity = GetComponent<TankEntity>();
        }

    }
}