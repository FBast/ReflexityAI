using System.Collections.Generic;
using System.Linq;
using Examples.CubeAI.Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static List<GameObject> Cubes;

    private void Start() {
        Cubes = FindObjectsOfType<CubeEntity>().Select(entity => entity.gameObject).ToList();
    }

}