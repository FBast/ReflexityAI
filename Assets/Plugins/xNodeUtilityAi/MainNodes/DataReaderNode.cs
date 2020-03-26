using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils;
using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.MainNodes {
    public class DataReaderNode : DataNode, IContextual {

        public AbstractAIComponent Context { get; set; }

        [HideInInspector] public List<ReflectionData> ReflectionDatas = new List<ReflectionData>();

        private void OnValidate() {
            if (graph is AIBrainGraph brainGraph && brainGraph.ContextType != null) {
                IEnumerable<ReflectionData> reflectionDatas = brainGraph.ContextType.Type.GetReflectionDatas(Context);
                ClearDynamicPorts();
                ReflectionDatas.Clear();
                foreach (ReflectionData reflectionData in reflectionDatas) {
                    AddDynamicOutput(reflectionData.Type, ConnectionType.Multiple, TypeConstraint.None, reflectionData.Name);
                    ReflectionDatas.Add(reflectionData);
                }
            }
        }

        protected override void Init() {
            base.Init();
            OnValidate();
        }

        public override object GetValue(NodePort port) {
            return ReflectionDatas.FirstOrDefault(reflectionData => reflectionData.Name == port.fieldName);
        }

    }


}


