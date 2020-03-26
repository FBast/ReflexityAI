using System.Collections.Generic;
using System.Linq;
using Examples.CubeAI.Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static List<CubeEntity> CubeEntities = new List<CubeEntity>();

    private void Start() {
        CubeEntities = FindObjectsOfType<CubeEntity>().ToList();
    }

}