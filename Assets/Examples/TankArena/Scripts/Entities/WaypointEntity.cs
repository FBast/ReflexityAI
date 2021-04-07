using System.Collections.Generic;
using System.Linq;
using Examples.TankArena.Scripts.Extensions;
using Examples.TankArena.Scripts.Framework;
using Examples.TankArena.Scripts.SOReferences.GameObjectListReference;
using UnityEngine;

namespace Examples.TankArena.Scripts.Entities {
    public class WaypointEntity : MonoBehaviour {

        [Header("Parameters")] 
        public LayerMask CoverLayer;
        
        [Header("SO References")]
        public GameObjectListReference TanksReference;
        
        public Transform Transform => transform;
        
        public int ObserverCount(TankEntity hider, FactionType seekersFaction) {
            if (seekersFaction == FactionType.All) {
                return transform.InLineOfView(hider.transform,
                    TanksReference.Value
                        .Select(o => o.GetComponent<TankEntity>())
                        .Where(entity => entity != hider)
                        .Select(entity => entity.transform).ToList(), CoverLayer).Count;
            }
            return transform.InLineOfView(hider.transform,
                TanksReference.Value
                    .Select(o => o.GetComponent<TankEntity>())
                    .Where(entity => entity != hider && hider.GetFaction(entity) == seekersFaction)
                    .Select(entity => entity.transform).ToList(), CoverLayer).Count;
        }

        public bool IsTargetObserver(TankEntity hider, TankEntity seeker) {
            if (seeker == null) return false;
            return transform.InLineOfView(hider.transform, new List<Transform> {seeker.transform}, CoverLayer).Count == 0;
        }
        
    }
}
