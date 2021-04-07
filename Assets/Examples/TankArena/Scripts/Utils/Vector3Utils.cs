using System.Collections.Generic;
using UnityEngine;

namespace Examples.TankArena.Scripts.Utils {
    public static class Vector3Utils {
		
        public static Vector3 RandomPositionBetween(Vector3 min, Vector3 max) {
            Vector3 randomPosition = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z) );
            return randomPosition;
        }
        
        public static Vector3 Average(List<Vector3> vector3s) {
            Vector3 averageVector = Vector2.zero;
            foreach (Vector3 vector3 in vector3s) {
                averageVector += vector3;
            }
            averageVector /= vector3s.Count;
            return averageVector;
        }
		
    }
}