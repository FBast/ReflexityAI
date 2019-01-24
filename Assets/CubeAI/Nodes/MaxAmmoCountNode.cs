using UtilityAI.Nodes;

namespace CubeAI.Nodes {
    public class MaxAmmoCountNode : SimpleEntryNode<CubeAiComponent> {

        public override void SetContext(CubeAiComponent context) {
            Value = context.CubeEntity.MaxAmmo;
        }
        
    }
}