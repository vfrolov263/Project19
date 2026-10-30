using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Rules.EventsCascade
{
    public class EventsCascadeController : MonoBehaviour
    {
        [SerializeField]
        private List<Object> _events;

        private Queue<IEvent> _eventsQueue = new();

        private void Awake()
        {
            foreach (var obj in _events)
            {
                if (obj is IEvent gameEvent)
                    _eventsQueue.Enqueue(gameEvent);
            }
        }

        private void OnEnable()
        {
            if (_eventsQueue.Count > 0)
                _eventsQueue.First().Finished += OnFinished;
            
            // foreach (var gameEvent in _eventsQueue)
            // {
            //     gameEvent.Finished += OnFinished;
            // }
        }

        private void OnDisable()
        {
            foreach (var gameEvent in _eventsQueue)
            {
                gameEvent.Finished -= OnFinished;
            }
        }

        private void OnFinished(IEvent _)
        {
            _eventsQueue.Dequeue().Finished -= OnFinished;

            if (_eventsQueue.Count > 0)
            {
                var gameEvent = _eventsQueue.Peek();
                gameEvent.Finished += OnFinished;
                gameEvent.Start();
            }
        }
    }
}