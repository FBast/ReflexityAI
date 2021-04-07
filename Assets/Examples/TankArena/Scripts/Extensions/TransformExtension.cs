using System.Collections.Generic;
using UnityEngine;

namespace Examples.TankArena.Scripts.Extensions {
    public static class TransformExtension {

        public static List<Transform> InLineOfView(this Transform trans, Transform Hider, List<Transform> Seekers, LayerMask layerMask) {
            List<Transform> inLineOfView = new List<Transform>();
            foreach (Transform seeker in Seekers) {
                if (Physics.Linecast(trans.position + Hider.PivotToCenter(), seeker.position + seeker.PivotToCenter(), layerMask)) continue;
                inLineOfView.Add(seeker);
            }
            return inLineOfView;
        }

        public static Vector3 PivotToCenter(this Transform trans) {
            Vector3 distance = Vector3.zero;
            if (trans.GetComponent<Collider>())
                distance = trans.GetComponent<Collider>().bounds.center - trans.position;
            return distance;
        }

    }
}