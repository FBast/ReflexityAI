using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace XNodeEditor {
    public partial class NodeEditorWindow {
        public enum NodeActivity { Idle, HoldNode, DragNode, HoldGrid, DragGrid }
        public static NodeActivity currentActivity = NodeActivity.Idle;
        public static bool isPanning { get; private set; }
        public static Vector2[] dragOffset;

        private bool IsDraggingPort { get { return draggedOutput != null; } }
        private bool IsHoveringPort { get { return hoveredPort != null; } }
        private bool IsHoveringNode { get { return hoveredNode != null; } }
        private bool IsHoveringReroute { get { return hoveredReroute.port != null; } }
        private XNode.Node hoveredNode = null;
        [NonSerialized] private XNode.NodePort hoveredPort = null;
        [NonSerialized] private XNode.NodePort draggedOutput = null;
        [NonSerialized] private XNode.NodePort draggedOutputTarget = null;
        [NonSerialized] private List<Vector2> draggedOutputReroutes = new List<Vector2>();
        private RerouteReference hoveredReroute = new RerouteReference();
        private List<RerouteReference> selectedReroutes = new List<RerouteReference>();
        private Rect nodeRects;
        private Vector2 dragBoxStart;
        private UnityEngine.Object[] preBoxSelection;
        private RerouteReference[] preBoxSelectionReroute;
        private Rect selectionBox;

        private struct RerouteReference {
            public XNode.NodePort port;
            public int connectionIndex;
            public int pointIndex;

            public RerouteReference(XNode.NodePort port, int connectionIndex, int pointIndex) {
                this.port = port;
                this.connectionIndex = connectionIndex;
                this.pointIndex = pointIndex;
            }

            public void InsertPoint(Vector2 pos) { port.GetReroutePoints(connectionIndex).Insert(pointIndex, pos); }
            public void SetPoint(Vector2 pos) { port.GetReroutePoints(connectionIndex) [pointIndex] = pos; }
            public void RemovePoint() { port.GetReroutePoints(connectionIndex).RemoveAt(pointIndex); }
            public Vector2 GetPoint() { return port.GetReroutePoints(connectionIndex) [pointIndex]; }
        }

        public void Controls() {
            wantsMouseMove = true;
            Event e = Event.current;
            switch (e.type) {
                case EventType.MouseMove:
                    break;
                case EventType.ScrollWheel:
                    if (e.delta.y > 0) zoom += 0.1f * zoom;
                    else zoom -= 0.1f * zoom;
                    break;
                case EventType.MouseDrag:
                    if (e.button == 0) {
                        if (IsDraggingPort) {
                            if (IsHoveringPort && hoveredPort.IsInput) {
                                if (!draggedOutput.IsConnectedTo(hoveredPort)) {
                                    draggedOutputTarget = hoveredPort;
                                }
                            } else {
                                draggedOutputTarget = null;
                            }
                            Repaint();
                        } else if (currentActivity == NodeActivity.HoldNode) {
                            RecalculateDragOffsets(e);
                            currentActivity = NodeActivity.DragNode;
                            Repaint();
                        }
                        if (currentActivity == NodeActivity.DragNode) {
                            // Holding ctrl inverts grid snap
                            bool gridSnap = NodeEditorPreferences.GetSettings().gridSnap;
                            if (e.control) gridSnap = !gridSnap;

                            Vector2 mousePos = WindowToGridPosition(e.mousePosition);
                            // Move selected nodes with offset
                            for (int i = 0; i < Selection.objects.Length; i++) {
                                if (Selection.objects[i] is XNode.Node) {
                                    XNode.Node node = Selection.objects[i] as XNode.Node;
                                    Vector2 initial = node.position;
                                    node.position = mousePos + dragOffset[i];
                                    if (gridSnap) {
                                        node.position.x = (Mathf.Round((node.position.x + 8) / 16) * 16) - 8;
                                        node.position.y = (Mathf.Round((node.position.y + 8) / 16) * 16) - 8;
                                    }

                                    // Offset portConnectionPoints instantly if a node is dragged so they aren't delayed by a frame.
                                    Vector2 offset = node.position - initial;
                                    if (offset.sqrMagnitude > 0) {
                                        foreach (XNode.NodePort output in node.Outputs) {
                                            Rect rect;
                                            if (portConnectionPoints.TryGetValue(output, out rect)) {
                                                rect.position += offset;
                                                portConnectionPoints[output] = rect;
                                            }
                                        }

                                        foreach (XNode.NodePort input in node.Inputs) {
                                            Rect rect;
                                            if (portConnectionPoints.TryGetValue(input, out rect)) {
                                                rect.position += offset;
                                                portConnectionPoints[input] = rect;
                                            }
                                        }
                                    }
                                }
                            }
                            // Move selected reroutes with offset
                            for (int i = 0; i < selectedReroutes.Count; i++) {
                                Vector2 pos = mousePos + dragOffset[Selection.objects.Length + i];
                                if (gridSnap) {
                                    pos.x = (Mathf.Round(pos.x / 16) * 16);
                                    pos.y = (Mathf.Round(pos.y / 16) * 16);
                                }
                                selectedReroutes[i].SetPoint(pos);
                            }
                            Repaint();
                        } else if (currentActivity == NodeActivity.HoldGrid) {
                            currentActivity = NodeActivity.DragGrid;
                            preBoxSelection = Selection.objects;
                            preBoxSelectionReroute = selectedReroutes.ToArray();
                            dragBoxStart = WindowToGridPosition(e.mousePosition);
                            Repaint();
                        } else if (currentActivity == NodeActivity.DragGrid) {
                            Vector2 boxStartPos = GridToWindowPosition(dragBoxStart);
                            Vector2 boxSize = e.mousePosition - boxStartPos;
                            if (boxSize.x < 0) { boxStartPos.x += boxSize.x; boxSize.x = Mathf.Abs(boxSize.x); }
                            if (boxSize.y < 0) { boxStartPos.y += boxSize.y; boxSize.y = Mathf.Abs(boxSize.y); }
                            selectionBox = new Rect(boxStartPos, boxSize);
                            Repaint();
                        }
                    } else if (e.button == 1 || e.button == 2) {
                        Vector2 tempOffset = panOffset;
                        tempOffset += e.delta * zoom;
                        // Round value to increase crispyness of UI text
                        tempOffset.x = Mathf.Round(tempOffset.x);
                        tempOffset.y = Mathf.Round(tempOffset.y);
                        panOffset = tempOffset;
                        isPanning = true;
                    }
                    break;
                case EventType.MouseDown:
                    Repaint();
                    if (e.button == 0) {
                        draggedOutputReroutes.Clear();

                        if (IsHoveringPort) {
                            if (hoveredPort.IsOutput) {
                                draggedOutput = hoveredPort;
                            } else {
                                hoveredPort.VerifyConnections();
                                if (hoveredPort.IsConnected) {
                                    XNode.Node node = hoveredPort.node;
                                    XNode.NodePort output = hoveredPort.Connection;
                                    int outputConnectionIndex = output.GetConnectionIndex(hoveredPort);
                                    draggedOutputReroutes = output.GetReroutePoints(outputConnectionIndex);
                                    hoveredPort.Disconnect(output);
                                    draggedOutput = output;
                                    draggedOutputTarget = hoveredPort;
                                    if (NodeEditor.onUpdateNode != null) NodeEditor.onUpdateNode(node);
                                }
                            }
                        } else if (IsHoveringNode && IsHoveringTitle(hoveredNode)) {
                            // If mousedown on node header, select or deselect
                            if (!Selection.Contains(hoveredNode)) {
                                SelectNode(hoveredNode, e.control || e.shift);
                                if (!e.control && !e.shift) selectedReroutes.Clear();
                            } else if (e.control || e.shift) DeselectNode(hoveredNode);
                            e.Use();
                            currentActivity = NodeActivity.HoldNode;
                        } else if (IsHoveringReroute) {
                            // If reroute isn't selected
                            if (!selectedReroutes.Contains(hoveredReroute)) {
                                // Add it
                                if (e.control || e.shift) selectedReroutes.Add(hoveredReroute);
                                // Select it
                                else {
                                    selectedReroutes = new List<RerouteReference>() { hoveredReroute };
                                    Selection.activeObject = null;
                                }

                            }
                            // Deselect
                            else if (e.control || e.shift) selectedReroutes.Remove(hoveredReroute);
                            e.Use();
                            currentActivity = NodeActivity.HoldNode;
                        }
                        // If mousedown on grid background, deselect all
                        else if (!IsHoveringNode) {
                            currentActivity = NodeActivity.HoldGrid;
                            if (!e.control && !e.shift) {
                                selectedReroutes.Clear();
                                Selection.activeObject = null;
                            }
                        }
                    }
                    break;
                case EventType.MouseUp:
                    if (e.button == 0) {
                        //Port drag release
                        if (IsDraggingPort) {
                            //If connection is valid, save it
                            if (draggedOutputTarget != null) {
                                XNode.Node node = draggedOutputTarget.node;
                                if (graph.nodes.Count != 0) draggedOutput.Connect(draggedOutputTarget);

                                // ConnectionIndex can be -1 if the connection is removed instantly after creation
                                int connectionIndex = draggedOutput.GetConnectionIndex(draggedOutputTarget);
                                if (connectionIndex != -1) {
                                    draggedOutput.GetReroutePoints(connectionIndex).AddRange(draggedOutputReroutes);
                                    if (NodeEditor.onUpdateNode != null) NodeEditor.onUpdateNode(node);
                                    EditorUtility.SetDirty(graph);
                                }
                            }
                            //Release dragged connection
                            draggedOutput = null;
                            draggedOutputTarget = null;
                            EditorUtility.SetDirty(graph);
                            if (NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
                        } else if (currentActivity == NodeActivity.DragNode) {
                            IEnumerable<XNode.Node> nodes = Selection.objects.Where(x => x is XNode.Node).Select(x => x as XNode.Node);
                            foreach (XNode.Node node in nodes) EditorUtility.SetDirty(node);
                            if (NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
                        } else if (!IsHoveringNode) {
                            // If click outside node, release field focus
                            if (!isPanning) {
                                EditorGUI.FocusTextInControl(null);
                            }
                            if (NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
                        }

                        // If click node header, select it.
                        if (currentActivity == NodeActivity.HoldNode && !(e.control || e.shift)) {
                            selectedReroutes.Clear();
                            SelectNode(hoveredNode, false);
                        }

                        // If click reroute, select it.
                        if (IsHoveringReroute && !(e.control || e.shift)) {
                            selectedReroutes = new List<RerouteReference>() { hoveredReroute };
                            Selection.activeObject = null;
                        }

                        Repaint();
                        currentActivity = NodeActivity.Idle;
                    } else if (e.button == 1 || e.button == 2) {
                        if (!isPanning) {
                            if (IsDraggingPort) {
                                draggedOutputReroutes.Add(WindowToGridPosition(e.mousePosition));
                            } else if (currentActivity == NodeActivity.DragNode && Selection.activeObject == null && selectedReroutes.Count == 1) {
                                selectedReroutes[0].InsertPoint(selectedReroutes[0].GetPoint());
                                selectedReroutes[0] = new RerouteReference(selectedReroutes[0].port, selectedReroutes[0].connectionIndex, selectedReroutes[0].pointIndex + 1);
                            } else if (IsHoveringReroute) {
                                ShowRerouteContextMenu(hoveredReroute);
                            } else if (IsHoveringPort) {
                                ShowPortContextMenu(hoveredPort);
                            } else if (IsHoveringNode && IsHoveringTitle(hoveredNode)) {
                                if (!Selection.Contains(hoveredNode)) SelectNode(hoveredNode, false);
                                ShowNodeContextMenu();
                            } else if (!IsHoveringNode) {
                                ShowGraphContextMenu();
                            }
                        }
                        isPanning = false;
                    }
                    break;
                case EventType.KeyDown:
                    if (EditorGUIUtility.editingTextField) break;
                    else if (e.keyCode == KeyCode.F) Home();
                    if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX) {
                        if (e.keyCode == KeyCode.Return) RenameSelectedNode();
                    } else {
                        if (e.keyCode == KeyCode.F2) RenameSelectedNode();
                    }
                    break;
                case EventType.ValidateCommand:
                    if (e.commandName == "SoftDelete") RemoveSelectedNodes();
                    else if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX && e.commandName == "Delete") RemoveSelectedNodes();
                    else if (e.commandName == "Duplicate") DublicateSelectedNodes();
                    Repaint();
                    break;
                case EventType.Ignore:
                    // If release mouse outside window
                    if (e.rawType == EventType.MouseUp && currentActivity == NodeActivity.DragGrid) {
                        Repaint();
                        currentActivity = NodeActivity.Idle;
                    }
                    break;
            }
        }

        private void RecalculateDragOffsets(Event current) {
            dragOffset = new Vector2[Selection.objects.Length + selectedReroutes.Count];
            // Selected nodes
            for (int i = 0; i < Selection.objects.Length; i++) {
                if (Selection.objects[i] is XNode.Node) {
                    XNode.Node node = Selection.objects[i] as XNode.Node;
                    dragOffset[i] = node.position - WindowToGridPosition(current.mousePosition);
                }
            }

            // Selected reroutes
            for (int i = 0; i < selectedReroutes.Count; i++) {
                dragOffset[Selection.objects.Length + i] = selectedReroutes[i].GetPoint() - WindowToGridPosition(current.mousePosition);
            }
        }

        /// <summary> Puts all nodes in focus. If no nodes are present, resets view to  </summary>
        public void Home() {
            zoom = 2;
            panOffset = Vector2.zero;
        }

        public void CreateNode(Type type, Vector2 position) {
            XNode.Node node = graph.AddNode(type);
            node.position = position;
            node.name = UnityEditor.ObjectNames.NicifyVariableName(type.Name);
            AssetDatabase.AddObjectToAsset(node, graph);
            if (NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
            Repaint();
        }

        /// <summary> Remove nodes in the graph in Selection.objects</summary>
        public void RemoveSelectedNodes() {
            // We need to delete reroutes starting at the highest point index to avoid shifting indices
            selectedReroutes = selectedReroutes.OrderByDescending(x => x.pointIndex).ToList();
            for (int i = 0; i < selectedReroutes.Count; i++) {
                selectedReroutes[i].RemovePoint();
            }
            selectedReroutes.Clear();
            foreach (UnityEngine.Object item in Selection.objects) {
                if (item is XNode.Node) {
                    XNode.Node node = item as XNode.Node;
                    graphEditor.RemoveNode(node);
                }
            }
        }

        /// <summary> Initiate a rename on the currently selected node </summary>
        public void RenameSelectedNode() {
            if (Selection.objects.Length == 1 && Selection.activeObject is XNode.Node) {
                XNode.Node node = Selection.activeObject as XNode.Node;
                NodeEditor.GetEditor(node).InitiateRename();
            }
        }

        /// <summary> Draw this node on top of other nodes by placing it last in the graph.nodes list </summary>
        public void MoveNodeToTop(XNode.Node node) {
            int index;
            while ((index = graph.nodes.IndexOf(node)) != graph.nodes.Count - 1) {
                graph.nodes[index] = graph.nodes[index + 1];
                graph.nodes[index + 1] = node;
            }
        }

        /// <summary> Dublicate selected nodes and select the dublicates </summary>
        public void DublicateSelectedNodes() {
            UnityEngine.Object[] newNodes = new UnityEngine.Object[Selection.objects.Length];
            Dictionary<XNode.Node, XNode.Node> substitutes = new Dictionary<XNode.Node, XNode.Node>();
            for (int i = 0; i < Selection.objects.Length; i++) {
                if (Selection.objects[i] is XNode.Node) {
                    XNode.Node srcNode = Selection.objects[i] as XNode.Node;
                    if (srcNode.graph != graph) continue; // ignore nodes selected in another graph
                    XNode.Node newNode = graphEditor.CopyNode(srcNode);
                    substitutes.Add(srcNode, newNode);
                    newNode.position = srcNode.position + new Vector2(30, 30);
                    newNodes[i] = newNode;
                }
            }

            // Walk through the selected nodes again, recreate connections, using the new nodes
            for (int i = 0; i < Selection.objects.Length; i++) {
                if (Selection.objects[i] is XNode.Node) {
                    XNode.Node srcNode = Selection.objects[i] as XNode.Node;
                    if (srcNode.graph != graph) continue; // ignore nodes selected in another graph
                    foreach (XNode.NodePort port in srcNode.Ports) {
                        for (int c = 0; c < port.ConnectionCount; c++) {
                            XNode.NodePort inputPort = port.direction == XNode.NodePort.IO.Input ? port : port.GetConnection(c);
                            XNode.NodePort outputPort = port.direction == XNode.NodePort.IO.Output ? port : port.GetConnection(c);

                            XNode.Node newNodeIn, newNodeOut;
                            if (substitutes.TryGetValue(inputPort.node, out newNodeIn) && substitutes.TryGetValue(outputPort.node, out newNodeOut)) {
                                newNodeIn.UpdateStaticPorts();
                                newNodeOut.UpdateStaticPorts();
                                inputPort = newNodeIn.GetInputPort(inputPort.fieldName);
                                outputPort = newNodeOut.GetOutputPort(outputPort.fieldName);
                            }
                            if (!inputPort.IsConnectedTo(outputPort)) inputPort.Connect(outputPort);
                        }
                    }
                }
            }
            Selection.objects = newNodes;
        }

        /// <summary> Draw a connection as we are dragging it </summary>
        public void DrawDraggedConnection() {
            if (IsDraggingPort) {
                Color col = NodeEditorPreferences.GetTypeColor(draggedOutput.ValueType);

                Rect fromRect;
                if (!_portConnectionPoints.TryGetValue(draggedOutput, out fromRect)) return;
                Vector2 from = fromRect.center;
                col.a = 0.6f;
                Vector2 to = Vector2.zero;
                for (int i = 0; i < draggedOutputReroutes.Count; i++) {
                    to = draggedOutputReroutes[i];
                    DrawConnection(from, to, col);
                    from = to;
                }
                to = draggedOutputTarget != null ? portConnectionPoints[draggedOutputTarget].center : WindowToGridPosition(Event.current.mousePosition);
                DrawConnection(from, to, col);

                Color bgcol = Color.black;
                Color frcol = col;
                bgcol.a = 0.6f;
                frcol.a = 0.6f;

                // Loop through reroute points again and draw the points
                for (int i = 0; i < draggedOutputReroutes.Count; i++) {
                    // Draw reroute point at position
                    Rect rect = new Rect(draggedOutputReroutes[i], new Vector2(16, 16));
                    rect.position = new Vector2(rect.position.x - 8, rect.position.y - 8);
                    rect = GridToWindowRect(rect);

                    NodeEditorGUILayout.DrawPortHandle(rect, bgcol, frcol);
                }
            }
        }

        bool IsHoveringTitle(XNode.Node node) {
            Vector2 mousePos = Event.current.mousePosition;
            //Get node position
            Vector2 nodePos = GridToWindowPosition(node.position);
            float width;
            Vector2 size;
            if (nodeSizes.TryGetValue(node, out size)) width = size.x;
            else width = 200;
            Rect windowRect = new Rect(nodePos, new Vector2(width / zoom, 30 / zoom));
            return windowRect.Contains(mousePos);
        }
    }
}