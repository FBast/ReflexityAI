using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins.xNodeUtilityAi.Framework {
    public static class AIQueue {
        
        internal static bool IsQueuing;
        internal static readonly Queue<ReflexityAI> Queue = new Queue<ReflexityAI>();

        internal static IEnumerator Queuing() {
            IsQueuing = true;
            while (IsQueuing) {
                yield return new WaitWhile(() => Queue.Count == 0);
                ReflexityAI dequeue = Queue.Dequeue();
                ReflexityAI peek = Queue.Peek();
                if (dequeue && dequeue.enabled && (!peek || peek != dequeue)) {
                    dequeue.ThinkAndAct();
                    if (dequeue.QueuingLoop) Queue.Enqueue(dequeue);
                    yield return null;
                }
            }
        }
        
    }
}
