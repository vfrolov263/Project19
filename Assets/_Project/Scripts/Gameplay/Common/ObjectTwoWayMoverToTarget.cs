using System;
using DG.Tweening;
using UnityEngine;
using Assets._Project.Scripts.Gameplay.Rules.EventsCascade;

namespace  Assets._Project.Scripts.Gameplay.Common
{
    public class ObjectTwoWayMoverToTarget : MonoBehaviour, IEvent
    {
        [SerializeField]
        private Vector3 _target;

        [SerializeField]
        private float _duration = 1f;

        [SerializeField]
        private bool _isAutoReturn;

        public event Action<IEvent> Started, Finished, Interrupted;

        private Vector3 _initPosition;

        private void Awake()
        {
            _initPosition = transform.localPosition;
        }

        void IEvent.Start()
        {
            enabled = true;
        }

        private void OnEnable()
        {
            transform.DOKill();
            TweenCallback onComplete = _isAutoReturn ? () => enabled = false : () => Finished?.Invoke(this);
            transform.DOLocalMove(_target, _duration).
                OnComplete(onComplete).
                OnKill(() => Interrupted?.Invoke(this));
        }

        private void OnDisable()
        {
            transform.DOKill();
            transform.DOLocalMove(_initPosition, _duration).
                OnComplete(() => Finished?.Invoke(this)).
                OnKill(() => Interrupted?.Invoke(this));
        }
    }
}