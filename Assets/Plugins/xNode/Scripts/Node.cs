﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace XNode {
    /// <summary>
    /// Base class for all nodes
    /// </summary>
    /// <example>
    /// Classes extending this class will be considered as valid nodes by xNode.
    /// <code>
    /// [System.Serializable]
    /// public class Adder : Node {
    ///     [Input] public float a;
    ///     [Input] public float b;
    ///     [Output] public float result;
    ///
    ///     // GetValue should be overridden to return a value for any specified output port
    ///     public override object GetValue(NodePort port) {
    ///         return a + b;
    ///     }
    /// }
    /// </code>
    /// </example>
    [Serializable]
    public abstract class Node : ScriptableObject {
        /// <summary> Used by <see cref="InputAttribute"/> and <see cref="OutputAttribute"/> to determine when to display the field value associated with a <see cref="NodePort"/> </summary>
        public enum ShowBackingValue {
            /// <summary> Never show the backing value </summary>
            Never,
            /// <summary> Show the backing value only when the port does not have any active connections </summary>
            Unconnected,
            /// <summary> Always show the backing value </summary>
            Always
        }

        public enum ConnectionType {
            /// <summary> Allow multiple connections</summary>
            Multiple,
            /// <summary> always override the current connection </summary>
            Override,
        }

        /// <summary> Iterate over all ports on this node. </summary>
        public IEnumerable<NodePort> Ports { get { foreach (NodePort port in ports.Values) yield return port; } }
        /// <summary> Iterate over all outputs on this node. </summary>
        public IEnumerable<NodePort> Outputs { get { foreach (NodePort port in Ports) { if (port.IsOutput) yield return port; } } }
        /// <summary> Iterate over all inputs on this node. </summary>
        public IEnumerable<NodePort> Inputs { get { foreach (NodePort port in Ports) { if (port.IsInput) yield return port; } } }
        /// <summary> Iterate over all instane ports on this node. </summary>
        public IEnumerable<NodePort> InstancePorts { get { foreach (NodePort port in Ports) { if (port.IsDynamic) yield return port; } } }
        /// <summary> Iterate over all instance outputs on this node. </summary>
        public IEnumerable<NodePort> InstanceOutputs { get { foreach (NodePort port in Ports) { if (port.IsDynamic && port.IsOutput) yield return port; } } }
        /// <summary> Iterate over all instance inputs on this node. </summary>
        public IEnumerable<NodePort> InstanceInputs { get { foreach (NodePort port in Ports) { if (port.IsDynamic && port.IsInput) yield return port; } } }
        /// <summary> Parent <see cref="NodeGraph"/> </summary>
        [SerializeField] public NodeGraph graph;
        /// <summary> Position on the <see cref="NodeGraph"/> </summary>
        [SerializeField] public Vector2 position;
        /// <summary> It is recommended not to modify these at hand. Instead, see <see cref="InputAttribute"/> and <see cref="OutputAttribute"/> </summary>
        [SerializeField] private NodePortDictionary ports = new NodePortDictionary();

        protected void OnEnable() {
            UpdateStaticPorts();
            Init();
        }

        /// <summary> Update static ports to reflect class fields. This happens automatically on enable. </summary>
        public void UpdateStaticPorts() {
            NodeDataCache.UpdatePorts(this, ports);
        }

        /// <summary> Initialize node. Called on creation. </summary>
        protected virtual void Init() { }

        /// <summary> Checks all connections for invalid references, and removes them. </summary>
        public void VerifyConnections() {
            foreach (NodePort port in Ports) port.VerifyConnections();
        }

#region Instance Ports
        /// <summary> Convenience function. </summary>
        /// <seealso cref="AddInstancePort"/>
        /// <seealso cref="AddInstanceOutput"/>
        public NodePort AddInstanceInput(Type type, Node.ConnectionType connectionType = Node.ConnectionType.Multiple, string fieldName = null) {
            return AddInstancePort(type, NodePort.IO.Input, connectionType, fieldName);
        }

        /// <summary> Convenience function. </summary>
        /// <seealso cref="AddInstancePort"/>
        /// <seealso cref="AddInstanceInput"/>
        public NodePort AddInstanceOutput(Type type, Node.ConnectionType connectionType = Node.ConnectionType.Multiple, string fieldName = null) {
            return AddInstancePort(type, NodePort.IO.Output, connectionType, fieldName);
        }

        /// <summary> Add a dynamic, serialized port to this node. </summary>
        /// <seealso cref="AddInstanceInput"/>
        /// <seealso cref="AddInstanceOutput"/>
        private NodePort AddInstancePort(Type type, NodePort.IO direction, Node.ConnectionType connectionType = Node.ConnectionType.Multiple, string fieldName = null) {
            if (fieldName == null) {
                fieldName = "instanceInput_0";
                int i = 0;
                while (HasPort(fieldName)) fieldName = "instanceInput_" + (++i);
            } else if (HasPort(fieldName)) {
                Debug.LogWarning("Port '" + fieldName + "' already exists in " + name, this);
                return ports[fieldName];
            }
            NodePort port = new NodePort(fieldName, type, direction, connectionType, this);
            ports.Add(fieldName, port);
            return port;
        }

        /// <summary> Remove an instance port from the node </summary>
        public void RemoveInstancePort(string fieldName) {
            RemoveInstancePort(GetPort(fieldName));
        }

        /// <summary> Remove an instance port from the node </summary>
        public void RemoveInstancePort(NodePort port) {
            if (port == null) throw new ArgumentNullException("port");
            else if (port.IsStatic) throw new ArgumentException("cannot remove static port");
            port.ClearConnections();
            ports.Remove(port.fieldName);
        }

        /// <summary> Removes all instance ports from the node </summary>
        [ContextMenu("Clear Instance Ports")]
        public void ClearInstancePorts() {
            List<NodePort> instancePorts = new List<NodePort>(InstancePorts);
            foreach (NodePort port in instancePorts) {
                RemoveInstancePort(port);
            }
        }
#endregion

#region Ports
        /// <summary> Returns output port which matches fieldName </summary>
        public NodePort GetOutputPort(string fieldName) {
            NodePort port = GetPort(fieldName);
            if (port == null || port.direction != NodePort.IO.Output) return null;
            else return port;
        }

        /// <summary> Returns input port which matches fieldName </summary>
        public NodePort GetInputPort(string fieldName) {
            NodePort port = GetPort(fieldName);
            if (port == null || port.direction != NodePort.IO.Input) return null;
            else return port;
        }

        /// <summary> Returns port which matches fieldName </summary>
        public NodePort GetPort(string fieldName) {
            NodePort port;
            if (ports.TryGetValue(fieldName, out port)) return port;
            else return null;
        }

        public bool HasPort(string fieldName) {
            return ports.ContainsKey(fieldName);
        }
#endregion

#region Inputs/Outputs
        /// <summary> Return input value for a specified port. Returns fallback value if no ports are connected </summary>
        /// <param name="fieldName">Field name of requested input port</param>
        /// <param name="fallback">If no ports are connected, this value will be returned</param>
        public T GetInputValue<T>(string fieldName, T fallback = default(T)) {
            NodePort port = GetPort(fieldName);
            if (port != null && port.IsConnected) return port.GetInputValue<T>();
            else return fallback;
        }

        /// <summary> Return all input values for a specified port. Returns fallback value if no ports are connected </summary>
        /// <param name="fieldName">Field name of requested input port</param>
        /// <param name="fallback">If no ports are connected, this value will be returned</param>
        public T[] GetInputValues<T>(string fieldName, params T[] fallback) {
            NodePort port = GetPort(fieldName);
            if (port != null && port.IsConnected) return port.GetInputValues<T>();
            else return fallback;
        }

        /// <summary> Returns a value based on requested port output. Should be overridden in all derived nodes with outputs. </summary>
        /// <param name="port">The requested port.</param>
        public virtual object GetValue(NodePort port) {
            Debug.LogWarning("No GetValue(NodePort port) override defined for " + GetType());
            return null;
        }
#endregion

        /// <summary> Called after a connection between two <see cref="NodePort"/>s is created </summary>
        /// <param name="from">Output</param> <param name="to">Input</param>
        public virtual void OnCreateConnection(NodePort from, NodePort to) { }

        /// <summary> Called after a connection is removed from this port </summary>
        /// <param name="port">Output or Input</param>
        public virtual void OnRemoveConnection(NodePort port) { }

        /// <summary> Disconnect everything from this node </summary>
        public void ClearConnections() {
            foreach (NodePort port in Ports) port.ClearConnections();
        }

        public override int GetHashCode() {
            return JsonUtility.ToJson(this).GetHashCode();
        }

        /// <summary> Mark a serializable field as an input port. You can access this through <see cref="GetInputPort(string)"/> </summary>
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
        public class InputAttribute : Attribute {
            public ShowBackingValue backingValue;
            public ConnectionType connectionType;
            public bool instancePortList;

            /// <summary> Mark a serializable field as an input port. You can access this through <see cref="GetInputPort(string)"/> </summary>
            /// <param name="backingValue">Should we display the backing value for this port as an editor field? </param>
            /// <param name="connectionType">Should we allow multiple connections? </param>
            public InputAttribute(ShowBackingValue backingValue = ShowBackingValue.Unconnected, ConnectionType connectionType = ConnectionType.Multiple, bool instancePortList = false) {
                this.backingValue = backingValue;
                this.connectionType = connectionType;
                this.instancePortList = instancePortList;
            }
        }

        /// <summary> Mark a serializable field as an output port. You can access this through <see cref="GetOutputPort(string)"/> </summary>
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
        public class OutputAttribute : Attribute {
            public ShowBackingValue backingValue;
            public ConnectionType connectionType;
            public bool instancePortList;

            /// <summary> Mark a serializable field as an output port. You can access this through <see cref="GetOutputPort(string)"/> </summary>
            /// <param name="backingValue">Should we display the backing value for this port as an editor field? </param>
            /// <param name="connectionType">Should we allow multiple connections? </param>
            public OutputAttribute(ShowBackingValue backingValue = ShowBackingValue.Never, ConnectionType connectionType = ConnectionType.Multiple, bool instancePortList = false) {
                this.backingValue = backingValue;
                this.connectionType = connectionType;
                this.instancePortList = instancePortList;
            }
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        public class CreateNodeMenuAttribute : Attribute {
            public string menuName;
            /// <summary> Manually supply node class with a context menu path </summary>
            /// <param name="menuName"> Path to this node in the context menu. Null or empty hides it. </param>
            public CreateNodeMenuAttribute(string menuName) {
                this.menuName = menuName;
            }
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        public class NodeTint : Attribute {
            public Color color;
            /// <summary> Specify a color for this node type </summary>
            /// <param name="r"> Red [0.0f .. 1.0f] </param>
            /// <param name="g"> Green [0.0f .. 1.0f] </param>
            /// <param name="b"> Blue [0.0f .. 1.0f] </param>
            public NodeTint(float r, float g, float b) {
                color = new Color(r, g, b);
            }

            /// <summary> Specify a color for this node type </summary>
            /// <param name="hex"> HEX color value </param>
            public NodeTint(string hex) {
                ColorUtility.TryParseHtmlString(hex, out color);
            }

            /// <summary> Specify a color for this node type </summary>
            /// <param name="r"> Red [0 .. 255] </param>
            /// <param name="g"> Green [0 .. 255] </param>
            /// <param name="b"> Blue [0 .. 255] </param>
            public NodeTint(byte r, byte g, byte b) {
                color = new Color32(r, g, b, byte.MaxValue);
            }
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        public class NodeWidth : Attribute {
            public int width;
            /// <summary> Specify a width for this node type </summary>
            /// <param name="width"> Width </param>
            public NodeWidth(int width) {
                this.width = width;
            }
        }

        [Serializable] private class NodePortDictionary : Dictionary<string, NodePort>, ISerializationCallbackReceiver {
            [SerializeField] private List<string> keys = new List<string>();
            [SerializeField] private List<NodePort> values = new List<NodePort>();

            public void OnBeforeSerialize() {
                keys.Clear();
                values.Clear();
                foreach (KeyValuePair<string, NodePort> pair in this) {
                    keys.Add(pair.Key);
                    values.Add(pair.Value);
                }
            }

            public void OnAfterDeserialize() {
                this.Clear();

                if (keys.Count != values.Count)
                    throw new System.Exception("there are " + keys.Count + " keys and " + values.Count + " values after deserialization. Make sure that both key and value types are serializable.");

                for (int i = 0; i < keys.Count; i++)
                    this.Add(keys[i], values[i]);
            }
        }
    }
}