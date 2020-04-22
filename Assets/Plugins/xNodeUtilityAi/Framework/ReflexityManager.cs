using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins.xNodeUtilityAi.Framework {
    public class ReflexityManager : MonoBehaviour {

        public static Queue<AbstractAIComponent> AbstractAiComponents = new Queue<AbstractAIComponent>();

        private void Start() {
            StartCoroutine(ProcessAI());
        }
        
        private IEnumerator ProcessAI() {
            while (true) {
                yield return new WaitWhile(() => AbstractAiComponents.Count == 0);
                AbstractAIComponent dequeue = AbstractAiComponents.Dequeue();
                if (dequeue && dequeue.enabled) {
                    yield return StartCoroutine(dequeue.ThinkAndAct());
                    AbstractAiComponents.Enqueue(dequeue);
                }
            }
        }
    
    }
}
