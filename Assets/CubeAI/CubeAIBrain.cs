using System;
using UnityEngine;
using UtilityAI;

namespace CubeAI {
    [Serializable, CreateAssetMenu(fileName = "CubeAiBrain", menuName = "xNode Examples/CubeAiBrain")]
    public class CubeAIBrain : UtilityAIBrain<CubeEntity> {}
}