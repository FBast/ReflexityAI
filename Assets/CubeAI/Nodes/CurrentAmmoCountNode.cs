using UtilityAI.Nodes;

namespace CubeAI.Nodes {
    public class CurrentAmmoCountNode : SimpleEntryNode<CubeAiComponent> {

        public override void SetContext(CubeAiComponent context) {
            Value = context.CubeEntity.CurrentAmmo;
        }
        
    }
}
