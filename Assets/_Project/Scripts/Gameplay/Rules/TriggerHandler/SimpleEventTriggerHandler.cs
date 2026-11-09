using System;
using Assets._Project.Scripts.Gameplay.Rules.EventsCascade;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Rules.TriggerHandler
{
    public class SimpleEventTriggerHandler : MonoBehaviour, IEvent
    {
        [SerializeField]
        private SimpleTriggerHandler _handler;

        public event Action<IEvent> Started;
        public event Action<IEvent> Finished;
        public event Action<IEvent> Interrupted;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void IEvent.Start()
        {
            Started?.Invoke(this);
            _handler.Execute();
            Finished?.Invoke(this);
        }

    }
}