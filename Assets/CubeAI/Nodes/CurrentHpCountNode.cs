using UtilityAI.Nodes;

namespace CubeAI.Nodes {
    public class CurrentHpCountNode : SimpleEntryNode<CubeEntity> {

        public override void SetContext(CubeEntity context) {
            Value = context.CurrentHp;
        }
    }
}