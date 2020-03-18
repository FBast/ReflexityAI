﻿using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.Framework {
    [Serializable, CreateAssetMenu(fileName = "UtilityAIBrain", menuName = "UtilityAI/AIBrainGraph")]
    public class AIBrainGraph : NodeGraph {
        
        [Header("Tags")]
        public List<string> HistoricTags = new List<string>();
        public List<string> MemoryTags = new List<string>();
        
        public List<T> GetNodes<T>() where T : Node {
            List<T> entryNodes = new List<T>();
            foreach (Node node in nodes) {
                if (node as T) 
                    entryNodes.Add((T) node);
            }
            return entryNodes;
        }

    }
}