using System.Collections.Generic;
using UnityEngine;

namespace Examples.TankArena.Scripts.SOReferences.GameObjectListReference {

    [CreateAssetMenu(fileName = "GameObjectList_Variable", menuName = "SOVariable/GameObjectList")]
    public class GameObjectListVariable : Variable<List<GameObject>> {

        public GameObjectListVariable() {
            Value = new List<GameObject>();
        }
        
    }
}