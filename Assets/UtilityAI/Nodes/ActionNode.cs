namespace UtilityAI.Nodes {
    public abstract class ActionNode<T> : ExitNode<T> {

        [Input(ShowBackingValue.Never)] public float Utility;
        
        public float GetValue() {
            return GetInputValue<float>("Utility");
        }

    }
}
