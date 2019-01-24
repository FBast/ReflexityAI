using XNode;

namespace UtilityAI.Nodes {

    public abstract class SimpleEntryNode<T> : EntryNode<T> {

        // The value of an output node field is not used for anything, but could be used for caching output results
        [Output] public float Value;
        
        // GetValue should be overridden to return a value for any specified output port
        public override object GetValue(NodePort port) {
            if (port.fieldName == "Value") {
                return Value;
            }
            return null;
        }
        


    }

}
