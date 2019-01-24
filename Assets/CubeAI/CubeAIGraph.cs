using System;
using UnityEngine;
using UtilityAI;

namespace CubeAI {
    [Serializable, CreateAssetMenu(fileName = "CubeAIGraph", menuName = "xNode Examples/CubeAIGraph")]
    public class CubeAIGraph : UtilityAIGraph<CubeAiComponent> { }
}