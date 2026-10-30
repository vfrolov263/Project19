using System;
using System.Collections;
using UnityEngine;
using Assets._Project.Scripts.Gameplay.Rules.EventsCascade;

namespace  Assets._Project.Scripts.Gameplay.Common
{
    public class SimpleAwaiter : MonoBehaviour, IEvent
    {
        [SerializeField]
        private float _delay;

        public event Action<IEvent> Started, Finished, Interrupted;

        void IEvent.Start()
        {
            enabled = true;
        }

        private void OnEnable() 
        {
            Started?.Invoke(this);
            StartCoroutine(AwaitRoutine());
        }

        private void OnDisable()
        {
            Finished?.Invoke(this);
        }

        private IEnumerator AwaitRoutine()
        {
            yield return new WaitForSeconds(_delay);
            enabled = false;
        }
    }
}