using System;
using System.Collections.Generic;
using Examples.TankArena.Scripts.Extensions;
using UnityEngine;

namespace Examples.TankArena.Scripts.Utils {
    public static class FormationUtils {
        
        /// <summary>
        /// Return a lined vector 2 formation
        /// </summary>
        /// <param name="actors">actors to formate</param>
        /// <param name="target">position for the formation</param>
        /// <param name="lineNumber">number of lines</param>
        /// <param name="distance">distance between each actors</param>
        /// <param name="randomizeDistance">randomization offset</param>
        /// <returns></returns>
        public static List<Vector3> Lined(List<GameObject> actors, Vector3 target, double lineNumber, float distance, int randomizeDistance = 0) {
            List<Vector3> positions = new List<Vector3>();
            double n = Math.Floor(actors.Count + lineNumber.Factorial() / lineNumber);
            for (int y = 0; y < lineNumber; y++) {
                for (int x = 0; x < n - y; x++) {
                    positions.Add(new Vector3(y + x * distance, 0, y * distance));
                }
            }
            positions = CenterFormation(positions);
            positions = RotateFormation(positions, actors, target);
            return randomizeDistance > 0 ? RandomizeFormation(positions, randomizeDistance) : positions;
        }
        
        /// <summary>
        /// Return a skirmish vector 2 formation
        /// </summary>
        /// <param name="actors">actors to formate</param>
        /// <param name="target">position for the formation</param>
        /// <param name="distance">distance between each actors</param>
        /// <returns></returns>
        public static List<Vector3> Skirmished(List<GameObject> actors, Vector3 target, float distance) {
            List<Vector3> positions = new List<Vector3>();
            double actorNumber = actors.Count;
            int n = 0;
            for (int y = 3; n < actorNumber; y++) {
                for (int x = y % 2; x < 3; x += 2) {
                    positions.Add(new Vector3(x * distance, 0, (y - 3) * distance));
                    n++;
                }
            }
            positions = CenterFormation(positions);
            return RotateFormation(positions, actors, target);
        }
        
        private static List<Vector3> CenterFormation(List<Vector3> positions) {
            Vector3 averageVector = Vector3Utils.Average(positions);
            List<Vector3> centeredPosition = new List<Vector3>();
            foreach (Vector3 position in positions) {
                centeredPosition.Add(position - averageVector);
            }
            return centeredPosition;
        }

        private static List<Vector3> RotateFormation(List<Vector3> positions, List<GameObject> actors, Vector3 target) {
            List<Vector3> turnedPositions = new List<Vector3>();
            Vector3 worldSpaceForward = actors[0].transform.InverseTransformDirection(Vector3.forward);
            Vector3 targetLocalDirection = actors[0].transform.InverseTransformPoint(target);
            float turnAngle = Vector3.Angle(worldSpaceForward, targetLocalDirection);
            foreach (Vector3 position in positions) {
                turnedPositions.Add(
                    position.RotatePointAroundPivot(new Vector3(0, 0, 0), new Vector3(0, turnAngle, 0)));
            }
            return turnedPositions;
        }

        private static List<Vector3> RandomizeFormation(List<Vector3> positions, int randomizeDistance) {
            List<Vector3> randomizedPosition = new List<Vector3>();
            foreach (Vector3 position in positions) {
                randomizedPosition.Add(Vector3Extension.RandomPositionBetween(
                    position - new Vector3(randomizeDistance, randomizeDistance),
                    position + new Vector3(randomizeDistance, randomizeDistance)));
            }
            return randomizedPosition;
        }
        
    }
}
