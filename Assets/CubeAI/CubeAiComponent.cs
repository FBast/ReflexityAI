using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilityAI.Nodes;
using Random = UnityEngine.Random;

namespace CubeAI {
    public class CubeAiComponent : MonoBehaviour {

        [Serializable]
        public class UtilityReasoner {

            public ActionNode<CubeAiComponent> CubeActionNode;
            public string ActionName;
            public float Utility;
            public float Weight;

            public UtilityReasoner(ActionNode<CubeAiComponent> cubeActionNode, float utility) {
                CubeActionNode = cubeActionNode;
                ActionName = cubeActionNode.name;
                Utility = utility;
            }
			
        }
        
        public CubeAIGraph CubeAiGraph;
        public float ProbabilityResult;
        public List<ActionNode<CubeAiComponent>> ActionNodes;
        public List<UtilityReasoner> UtilityReasoners;
        
        // External References
        public CubeEntity CubeEntity;
        
        private void Start() {
            CubeEntity = GetComponent<CubeEntity>();
            InvokeRepeating("ThinkAndAct", 0, 0.5f);
        }
        
        public void ThinkAndAct() {
            UtilityReasoner utilityReasoner = ChooseAction();
            if (utilityReasoner == null) return;
//            Debug.Log(utilityReasoner.CubeActionNode.name 
//                      + " : Utility [" + utilityReasoner.Utility 
//                      + "] Weight [" + utilityReasoner.Weight + "]");
            utilityReasoner.CubeActionNode.Execute(this);
        }
        
        public UtilityReasoner ChooseAction() {
            if (CubeAiGraph == null) return null;
            CubeAiGraph.GetEntryNode().ForEach(node => node.SetContext(this));
            ActionNodes = CubeAiGraph.GetActionNode().ToList();
            // Fill the Utility Reasoner
            UtilityReasoners = new List<UtilityReasoner>();
            ActionNodes.ForEach(node => UtilityReasoners.Add(new UtilityReasoner(node, node.GetValue())));
            // Remove ImpossibleDecisionValue Utilities
            UtilityReasoners.RemoveAll(reasoner => reasoner.Utility <= 0f);
            // If no more decision then return
            if (UtilityReasoners.Count == 0)
                return null;
            // Calculating Weights
            foreach (UtilityReasoner utilityReasoner in UtilityReasoners) {
                utilityReasoner.Weight = utilityReasoner.Utility / UtilityReasoners.Sum(reasoner => reasoner.Utility);
            }
            // Sorting by Weights values
            UtilityReasoners = UtilityReasoners.OrderByDescending(tuple => tuple.Weight).ToList();
            ProbabilityResult = Random.Range(0f, 1f);
            float weightSum = 0f;
            foreach (UtilityReasoner utilityReasoner in UtilityReasoners) {
                weightSum += utilityReasoner.Weight;
                if (weightSum >= ProbabilityResult)
                    return utilityReasoner;
            }
            return null;
        }

    }

}
