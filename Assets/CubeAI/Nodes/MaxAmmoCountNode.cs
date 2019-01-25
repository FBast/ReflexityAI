using UtilityAI.Nodes;

namespace CubeAI.Nodes {
    public class MaxAmmoCountNode : SimpleEntryNode<CubeEntity> {

        public override void SetContext(CubeEntity context) {
            Value = context.MaxAmmo;
        }
        
    }
}