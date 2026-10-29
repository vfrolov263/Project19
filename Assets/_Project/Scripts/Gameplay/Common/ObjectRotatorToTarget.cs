using System;
using Assets._Project.Scripts.Gameplay.Rules.EventsCascade;
using DG.Tweening;
using UnityEngine;

namespace  Assets._Project.Scripts.Gameplay.Common
{
    public class ObjectRotatorToTarget : MonoBehaviour, IEvent
    {
        [SerializeField]
        private Vector3 targetAngles;

        [SerializeField]
        private float _duration;

        public event Action<IEvent> Started, Finished, Interrupted;

        void IEvent.Start()
        {
            this.enabled = true;
        }

        private void OnEnable()
        {
            Started?.Invoke(this);
            transform.DOLocalRotate(targetAngles, _duration).
                OnComplete(() => Finished?.Invoke(this)).
                OnKill(() => Interrupted?.Invoke(this));
        }
    }
}