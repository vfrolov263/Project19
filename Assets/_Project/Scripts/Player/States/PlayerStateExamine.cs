using Assets._Project.Scripts.Gameplay.Pausable;
using Assets._Project.Scripts.Player.Controlls;
using Assets._Project.Scripts.Player.Interaction;
using System;
using UnityEngine;

namespace Assets._Project.Scripts.Player.States
{
    public class PlayerStateExamine : IPlayerState
    {
        private FirstPersonInputController _inputController;
        protected PlayerController _playerController;
        private InteractionHandler _interactionHandler;
        private IInteractable _interactable;

        private bool _isPaused;

        public virtual PlayerState State => PlayerState.Examine;

        public event Action<PlayerActionMessage> Actioned;

        public bool Paused
        {
            get => _isPaused;
            set
            {
                if (value)
                    Pause();
                else
                    Continue();
            }
        }

        public PlayerStateExamine(PlayerController playerController, FirstPersonInputController inputController,
            IInteractable interactable)
        {
            _playerController = playerController;
            _inputController = inputController;   
            _interactable = interactable;
            _interactable.ExitRequested += OnExitRequested;
        }

        public void Dispose() => Stop();

        public void Start()
        {
            _inputController.CharacterControllerActivity = false;
            _inputController.ExamineControllerActivity = true;
            Paused = _playerController.Paused;

            if (_interactable.IsNeedPointer)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            // if (_interactable != null)
            // {
            //     _interactable.Interact();
            // }
        }

        public void Stop() => Pause();

        protected virtual void Pause()
        {
            _inputController.Examined -= OnExamined;
            _inputController.Exited -= OnExited;
            _isPaused = true;
        }

        protected virtual void Continue()
        {
            if (_interactable.HasToDeepExamine)
                _inputController.Examined += OnExamined;

            if (_interactable.HasManualExit)
                _inputController.Exited += OnExited;

            _isPaused = false;
        }

        private void OnExamined()
        {
            Ray ray = Camera.main.ScreenPointToRay(_inputController.PointerPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                if (interactable != null && interactable.HasToInteract)
                {
                    interactable.Interact();
                }
            }
        }

        private void OnExitRequested()
        {
            OnExited();
        }

        protected virtual void OnExited()
        {
            if (_interactable != null)
            {
                Actioned?.Invoke(new() { Interactable = _interactable, ActionType = PlayerAction.ExitInteract });
                _interactable.ExitRequested -= OnExitRequested;
                _interactable.ExitInteraction(() => _playerController.ChangeState(PlayerState.Walk));
            }
        }
    }
}