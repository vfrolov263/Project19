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

        [field: SerializeField]
        public bool IsFrontPusher { get; private set; }

        public bool IsOpened => _doorController.State == DoorState.OpenedBack || _doorController.State == DoorState.OpenedFront;

        public bool HasToExamine => false;

        private Collider _collider;
        private CinemachineBrain _camBrain;

        public event Action ExitRequested;        

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

        private void OnClosed() 
        {
            Closed?.Invoke();
            _doorController.PlayCloseSound();
        }

        public bool Interact()
        {
            if (_doorController.State == DoorState.Moving)
                return false;

            if (_doorController.IsLocked)
            {
                _doorController.PullHandle();
                _doorController.PlayLockedSound();
                return false;
            }
                
            _collider.enabled = false;
            _doorController.OpenHandle();

            if (_doorController.State == (IsFrontPusher ? DoorState.OpenedBack : DoorState.OpenedFront))
            {
                _doorController.Close();
                _doorController.PlayCloseSound();
                NeedManualControl = false;
            }
            else
            {
                _doorCamera.enabled = true;
                _doorController.PlayOpenSound();
                NeedManualControl = true;
            }

            return true;
        }

        public void Push(float force)
        {
            if (force >= 0f)
                _doorController.OpenBy(IsFrontPusher ? -force : force);
            else
                _doorController.CloseBy(IsFrontPusher ? force : -force);
        }

        public void ExitInteraction(Action onDone = null) => _ = ExitRoutine(onDone);

        private async Awaitable ExitRoutine(Action onDone)
        {
            _doorController.CloseHandle();
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