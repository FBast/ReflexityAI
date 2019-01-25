using UtilityAI.Nodes;

namespace CubeAI.Nodes {
    public class CurrentAmmoCountNode : SimpleEntryNode<CubeEntity> {

        public override void SetContext(CubeEntity context) {
            Value = context.CurrentAmmo;
        }
        
    }
}
