using UnityEngine;
using UnityEngine.AI;

namespace Examples.TankArena.Scripts.Extensions {
	public static class Vector3Extension {
		
		public static Vector3 RotatePointAroundPivot(this Vector3 point, Vector3 pivot, Vector3 angles) {
			return Quaternion.Euler(angles) * (point - pivot) + pivot;
		}

		public static Vector3 RandomPositionBetween(Vector3 min, Vector3 max) {
			Vector3 randomPosition = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z) );
			return randomPosition;
		}
		
		public static bool IsPositionOnNavMesh(this Vector3 position) {
			const float onMeshThreshold = 1;
			// Check for nearest point on navmesh to agent, within onMeshThreshold
			return NavMesh.SamplePosition(position, out _, onMeshThreshold, NavMesh.AllAreas);
		}
		
	}
}