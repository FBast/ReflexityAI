using System;
using System.Collections.Generic;
using UnityEngine;

namespace Examples.TankArena.Scripts.SOReferences.GameObjectListReference {
    [Serializable]
    public class GameObjectListReference : Reference<List<GameObject>, GameObjectListVariable> {}
}