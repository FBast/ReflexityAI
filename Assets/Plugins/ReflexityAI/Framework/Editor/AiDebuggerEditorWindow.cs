using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Plugins.ReflexityAI.Framework.Editor {
    public class AiDebuggerEditorWindow : EditorWindow {
        
        private GameObject _currentGameObject;
        private ReflexityAI _reflexityAi;
        private Gradient _weightGradiant;
        
        [MenuItem("Tool/AI Debugger")]
        private static void Init() {
            AiDebuggerEditorWindow window = GetWindow<AiDebuggerEditorWindow>("AI Debugger", true);
            window.Show();
        }

        private void Awake() {
            _weightGradiant = new Gradient {
                colorKeys = new [] {
                    new GradientColorKey(Color.green, 1),
                    new GradientColorKey(Color.red, 0) 
                },
                alphaKeys = new [] {
                    new GradientAlphaKey(0.25f, 1),
                    new GradientAlphaKey(0.25f, 0)
                }
            };
        }

        private void OnInspectorUpdate() {
            Repaint();
        }

        private void OnSelectionChange() {
            // New selection control
            if (Selection.activeGameObject != _currentGameObject || _currentGameObject == null) {
                // Component control
                if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<ReflexityAI>() == null) return;
                // Update debug data
                _currentGameObject = Selection.activeGameObject;
                _reflexityAi = _currentGameObject.GetComponent<ReflexityAI>();
            }
        }

        private void OnGUI() {
            GUIStyle labelGuiStyle = new GUIStyle(GUI.skin.label) {
                alignment = TextAnchor.MiddleCenter
            };
            if (!EditorApplication.isPlaying) {
                EditorGUI.LabelField(new Rect(0, 0, position.width, position.height), "The AI Debugger is only available in play mode", labelGuiStyle);
            } else if (_reflexityAi == null) {
                EditorGUI.LabelField(new Rect(0, 0, position.width, position.height), "Please select a GameObject with an AbstractAIBrain derived Component", labelGuiStyle);
            } else if (!_reflexityAi.enabled) {
                EditorGUI.LabelField(new Rect(0, 0, position.width, position.height), "It seems that the last selected AbstractAIBrain is sleeping, please select another", labelGuiStyle);
            } else if (_reflexityAi.WeightedOptions.Count == 0) {
                EditorGUI.LabelField(new Rect(0, 0, position.width, position.height), "No options found for " + _currentGameObject.name, labelGuiStyle);
            }
            else {
                // Display options
                int columnNumber = _reflexityAi.WeightedOptions.Count;
                float columnWidth = (position.width - 12) / columnNumber;
                float rowHeight = 20;
                int i = 0;
                EditorGUI.LabelField(new Rect(3, 3, position.width - 6, rowHeight), "Brain of " + _currentGameObject.name, labelGuiStyle);
                foreach (KeyValuePair<AIBrainGraph,List<AIOption>> valuePair in _reflexityAi.WeightedOptions) {
                    float weightMax = valuePair.Value.Max(option => option.Weight);
                    float weightMin = valuePair.Value.Min(option => option.Weight);
                    EditorGUI.LabelField(new Rect(3 + i * (columnWidth + 6), 3 + rowHeight, columnWidth, rowHeight), valuePair.Key.name, labelGuiStyle);
                    for (int j = 0; j < valuePair.Value.Count; j++) {
                        EditorGUI.ProgressBar(new Rect(3 + i * (columnWidth + 6), 3 + (j + 2) * rowHeight, columnWidth, rowHeight), 
                            valuePair.Value[j].Probability, valuePair.Value[j].ToString());
                        Color weightColor;
                        // Option with disqualified weight
                        if (valuePair.Value[j].Weight == 0) {
                            weightColor = new Color(1, 1, 1, 0.5f);
                        } 
                        // Part of the selected options
                        else if (_reflexityAi.SelectedOptions.ContainsKey(valuePair.Key) && valuePair.Value[j] 
                                 == _reflexityAi.SelectedOptions[valuePair.Key]) {
                            weightColor = new Color(0, 0, 0, 0.5f);
                        } 
                        // All options have the same weight
                        else if (Math.Abs(weightMax - weightMin) <= 0) {
                            weightColor = new Color(0, 1, 0, 0.25f);
                        }
                        // Option with valid weight
                        else {
                            float weightAbs = valuePair.Value[j].Weight * (weightMax - weightMin) / weightMax;
                            float colorTime = weightAbs / (weightMax - weightMin);
                            weightColor = _weightGradiant.Evaluate(colorTime);
                        }
                        EditorGUI.DrawRect(new Rect(3 + i * (columnWidth + 6), 3 + (j + 2) * rowHeight, columnWidth, rowHeight), weightColor);
                    }
                    i++;
                }
            }
        }

    }
}
