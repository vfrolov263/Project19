using System;
using Assets._Project.Scripts.Player.Interaction;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;

namespace Assets._Project.Scripts.Gameplay.Props
{
    public class LockWheelInteractable : MonoBehaviour, IInteractable
    {
        public string Name => "";
        public bool HasToExamine => false;
        public bool HasToInteract => true;

        [SerializeField]
        private float _angleDelta;

        [SerializeField]
        private int _correctDigit, _currentDigit = 0;

        private int _digits;

        [field: SerializeField]
        public float InteractionDistance {get; private set; } = 0.5f;

        public virtual event Action ExitRequested;

        private void Awake()
        {
            _digits = (int)MathF.Ceiling(360f / _angleDelta);
        }

        public bool Interact()
        {
            transform.Rotate(_angleDelta, 0, 0, Space.Self);
            _currentDigit++;

            if (_currentDigit >= _digits)
                _currentDigit = 0;

            Changed?.Invoke(this);
            return false;
        }

        public bool IsCorrectPosition => _correctDigit == _currentDigit;

        public event Action<LockWheelInteractable> Changed;
    }
}