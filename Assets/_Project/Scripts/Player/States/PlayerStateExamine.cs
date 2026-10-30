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
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

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
            _inputController.Examined += OnExamined;
            _inputController.Exited += OnExited;
            _isPaused = false;
        }

        private void OnExamined()
        { 
        }

        private void OnExitRequested()
        {
            OnExited();
        }

        protected virtual void OnExited()
        {
            if (_interactable != null)
            {
                _interactable.ExitRequested -= OnExitRequested;
                _interactable.ExitInteraction(() => _playerController.ChangeState(PlayerState.Walk));
            }
        }
    }
}