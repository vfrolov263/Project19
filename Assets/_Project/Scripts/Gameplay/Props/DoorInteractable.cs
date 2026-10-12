using System;
using Assets._Project.Scripts.Player;
using Assets._Project.Scripts.Player.Interaction;
using Assets._Project.Scripts.Player.States;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Props
{
    [RequireComponent(typeof(Collider))]
    public class DoorInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private DoorController _doorController;
        public string Name { get => "Ручка"; }
        public bool NeedManualControl { get; private set; }

        public event Action Opened, Closed;

        [field: SerializeField]
        public Transform OpenerPosition { get; private set; }
        [field: SerializeField]
        public Transform CloserPosition { get; private set; }

        [field: SerializeField]
        private CinemachineCamera _doorCamera;

        [SerializeField]
        private bool _isFrontPusher;

        private Collider _collider;
        private CinemachineBrain _camBrain;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _camBrain = Camera.main.GetComponent<CinemachineBrain>();
        }

        private void OnEnable()
        {
            _doorController.Opened += OnOpened;
            _doorController.Closed += OnClosed;
        }

        private void OnDisable()
        {
            _doorController.Opened -= OnOpened;
            _doorController.Closed -= OnClosed;
        }

        private void OnOpened() => Opened?.Invoke();

        private void OnClosed() => Closed?.Invoke();

        public void Interact()
        {
            if (_doorController.State == DoorState.Moving)
                return;
                
            _collider.enabled = false;

            if (_doorController.State == (_isFrontPusher ? DoorState.OpenedBack : DoorState.OpenedFront))
            {
                _doorController.Close();
                NeedManualControl = false;
            }
            else
            {
                _doorCamera.enabled = true;
                NeedManualControl = true;
            }
        }

        public void Push(float force)
        {
            if (force >= 0f)
                _doorController.OpenBy(_isFrontPusher ? -force : force);
            else
                _doorController.CloseBy(_isFrontPusher ? force : -force);
        }

        public void ExitInteraction(Action onDone = null) => _ = ExitRoutine(onDone);

        private async Awaitable ExitRoutine(Action onDone)
        {
            _doorCamera.enabled = false;
            await Awaitable.NextFrameAsync(destroyCancellationToken);

            while (_camBrain != null && _camBrain.IsBlending)
            {
                await Awaitable.FixedUpdateAsync(destroyCancellationToken);
            }

            _collider.enabled = true;
            onDone?.Invoke();
        }
    }
}