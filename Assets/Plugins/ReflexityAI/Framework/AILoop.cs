using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins.ReflexityAI.Framework {
    public class AILoop {

        public bool IsPaused;
        
        private bool _hasStarted;
        private readonly Queue<ReflexityAI> _queue = new Queue<ReflexityAI>();
        private readonly List<ReflexityAI> _dequeued = new List<ReflexityAI>();

        public void Start(MonoBehaviour monoBehaviour) {
            if (_hasStarted) return;
            _hasStarted = true;
            monoBehaviour.StartCoroutine(Looping());
        }

        public void Stop() {
            _hasStarted = false;
        }
        
        private IEnumerator Looping() {
            while (_hasStarted) {
                yield return new WaitWhile(() => IsPaused);
                yield return new WaitWhile(() => _queue.Count == 0);
                ReflexityAI dequeue = _queue.Dequeue();
                Debug.Log("Processing : " + dequeue.name);
                if (_dequeued.Contains(dequeue)) {
                    _dequeued.Remove(dequeue);
                }
                else if (dequeue && dequeue.enabled) {
                    dequeue.ThinkAndAct();
                    _queue.Enqueue(dequeue);
                    yield return null;
                }
            }
        }

        public void EnloopAI(ReflexityAI reflexityAI) {
            if (_queue.Contains(reflexityAI)) {
                Debug.Log("Queue already contains " + reflexityAI.name + " AI");
            } 
            else {
                _queue.Enqueue(reflexityAI);
            }
        }

        public void DeloopAI(ReflexityAI reflexityAI) {
            if (!_queue.Contains(reflexityAI)) {
                Debug.Log("Queue does not contains " + reflexityAI.name + " AI");
            }
            else if (_dequeued.Contains(reflexityAI)) {
                Debug.Log("AI " + reflexityAI.name + " is already in dequeue process");
            }
            else {
                _dequeued.Add(reflexityAI);
            }
        }
        
        public bool Contains(ReflexityAI reflexityAI) {
            return _queue.Contains(reflexityAI);
        }

        public void Clear() {
            _queue.Clear();
            _dequeued.Clear();
        }

    }
}
