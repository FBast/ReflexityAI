using System.Collections.Generic;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.MainNodes {
    public class DataSelectorNode : DataNode {
        
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] public Object Data;
        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] public Object Output;

        public ReflectionData SelectedReflectionData;
        public List<ReflectionData> ReflectionDatas = new List<ReflectionData>();

        private void OnValidate() {
            ReflectionDatas.Clear();
            ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(Data));
            foreach (ReflectionData innerReflectionData in reflectionData.Type.GetReflectionDatas()) {
                ReflectionDatas.Add(innerReflectionData);
            }
        }
        
        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            if (to.fieldName == nameof(Data) && to.node == this) {
                ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(Data));
                foreach (ReflectionData innerReflectionData in reflectionData.Type.GetReflectionDatas()) {
                    ReflectionDatas.Add(innerReflectionData);
                }
            }
        }
        
        public override void OnRemoveConnection(NodePort port) {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(Data) && port.node == this) {
                ReflectionDatas.Clear();
            }
        }
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(Output)) {
                ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(Data));
                object data = null;
                if (reflectionData.Data != null) {
                    data = SelectedReflectionData.Type.GetValue(reflectionData.Data);
                }
                return new ReflectionData(SelectedReflectionData.Name, SelectedReflectionData.Type, data);
            }
            return null;
        }

    }
}