using Assets._Project.Scripts.Gameplay.Pausable;
using Assets._Project.Scripts.Player.Controlls;
using Assets._Project.Scripts.Player.Interaction;
using UnityEngine;

namespace Assets._Project.Scripts.Player.States
{
    public class PlayerStateExamine : IPlayerState
    {
        private FirstPersonInputController _inputController;
        private PlayerController _playerController;
        private InteractionHandler _interactionHandler;
        private IInteractable _interactable;

        private bool _isPaused;

        public PlayerState State => PlayerState.Examine;

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
        }

        public void Dispose() => Stop();

        public void Start()
        {
            _inputController.CharacterControllerActivity = false;
            _inputController.ExamineControllerActivity = true;
            Paused = _playerController.Paused;

            if (_interactable != null)
            {
                _interactable.Interact();
            }
        }

        public void Stop() => Pause();

        private void Pause()
        {
            _inputController.Examined -= OnExamined;
            _inputController.Exited -= OnExited;
            _isPaused = true;
        }

        private void Continue()
        {
            _inputController.Examined += OnExamined;
            _inputController.Exited += OnExited;
            _isPaused = false;
        }

        private void OnExamined()
        { 
        }

        private void OnExited()
        {
            if (_interactable != null)
            {
                _interactable.ExitInteraction(() => _playerController.ChangeState(PlayerState.Walk));
            }
        }
    }
}