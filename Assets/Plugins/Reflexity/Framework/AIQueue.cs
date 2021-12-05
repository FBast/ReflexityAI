using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins.Reflexity.Framework {
    public class AIQueue {

        internal bool IsPaused;
        internal bool IsStopped;

        internal readonly Queue<ReflexityAI> Queue = new Queue<ReflexityAI>();
        internal readonly List<ReflexityAI> Dequeued = new List<ReflexityAI>();

        internal IEnumerator Queuing() {
            while (!IsStopped) {
                yield return new WaitWhile(() => IsPaused);
                yield return new WaitWhile(() => Queue.Count == 0);
                ReflexityAI dequeue = Queue.Dequeue();
                if (Dequeued.Contains(dequeue)) {
                    Dequeued.Remove(dequeue);
                } 
                else {
                    ReflexityAI peek = Queue.Count > 0 ? Queue.Peek() : null;
                    if (dequeue && dequeue.enabled && (!peek || peek != dequeue)) {
                        dequeue.ThinkAndAct();
                        Queue.Enqueue(dequeue);
                        yield return null;
                    }
                }
            }
            Queue.Clear();
            Dequeued.Clear();
        }

    }
}
