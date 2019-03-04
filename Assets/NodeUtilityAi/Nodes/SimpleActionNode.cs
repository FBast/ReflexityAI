using NodeUtilityAi.Framework;

namespace NodeUtilityAi.Nodes {
    public abstract class SimpleActionNode : ActionNode {

        [Input(ShowBackingValue.Never)] public TaggedData Data;
        
    }

}