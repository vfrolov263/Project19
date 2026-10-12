using System;
using DG.Tweening;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Props
{
    public enum DoorState
    {
        Closed,
        OpenedFront,
        OpenedBack,
        Moving
    }

    public class DoorController : MonoBehaviour
    {
        [Header("Door Settings")]
        [SerializeField]
        private Transform _doorTransform;

        [SerializeField]
        private Vector3 _hingeAxis = Vector3.up;

        [SerializeField]
        private float _closedAngle = 0f;

        [SerializeField]
        private float _fullyOpenAngle = 90f;

        [Header("Thresholds")]
        [SerializeField]
        private float _openThreshold = 70f;

        [SerializeField]
        private float _closeThreshold = 20f;

        [Header("Positions")]
        [SerializeField]
        private Transform _playerStandTransform;

        [Header("Movement")]
        [SerializeField]
        private float _moveSensitivity = 5f;

        private float _currentAngle;

        [field: SerializeField]
        public DoorState State { get; private set;}

        public event Action Opened;
        public event Action Closed;


        private void Awake()
        {
            if (_doorTransform == null)
                _doorTransform = transform;

            _currentAngle = State switch
            {
                DoorState.Closed => _closedAngle,
                DoorState.OpenedFront => -_fullyOpenAngle,
                DoorState.OpenedBack => _fullyOpenAngle,
                _ => 0f
            };

            ApplyRotation();
        }

        public float GetCurrentAngle() => _currentAngle;

        public void SetAngle(float angle)
        {
            _currentAngle = Mathf.Clamp(angle, -_fullyOpenAngle, _fullyOpenAngle);
            ApplyRotation();
        }

        public void OpenBy(float delta)
        { 
            SetAngle(_currentAngle + delta * _moveSensitivity);
            TryAutoOpen();
        }

        public void CloseBy(float delta) 
        {
            SetAngle(_currentAngle - delta * _moveSensitivity);
            TryAutoClose();
        }

        public void TryAutoOpen()
        {
            if (Mathf.Abs(_currentAngle) >= _openThreshold)
            {
                UseDoorCloser(_currentAngle != 0f ?  _currentAngle / Mathf.Abs(_currentAngle) * _fullyOpenAngle : 0f, 
                    _currentAngle < 0f ? DoorState.OpenedFront : DoorState.OpenedBack);
                Opened?.Invoke();
            }
        }

        public void TryAutoClose()
        {
            if (Mathf.Abs(_currentAngle) <= _closeThreshold)
            {
                UseDoorCloser(_currentAngle != 0f ?  _currentAngle / Mathf.Abs(_currentAngle) * _closedAngle : 0f, DoorState.Closed);
                Closed?.Invoke();
            }
        }

        public void Close() => UseDoorCloser(_closedAngle, DoorState.Closed);

        private void ApplyRotation()
        {
            _doorTransform.localRotation = Quaternion.AngleAxis(_currentAngle, _hingeAxis);
        }

        private void UseDoorCloser(float toAngle, DoorState finalState)
        {
            State = DoorState.Moving;
            _doorTransform.DOLocalRotateQuaternion(Quaternion.AngleAxis(toAngle, _hingeAxis), 1f)
            .OnComplete(() =>
            {
                State = finalState;
                SetAngle(toAngle);
            });
        }

        public Vector3 GetForwardDirection() =>
            _playerStandTransform != null ? _playerStandTransform.forward : transform.forward;

        public Transform PlayerStandTransform => _playerStandTransform;
        public float MoveSensitivity => _moveSensitivity;
    }
}