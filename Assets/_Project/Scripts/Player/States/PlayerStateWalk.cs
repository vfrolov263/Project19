using Assets._Project.Scripts.Gameplay.Pausable;
using Assets._Project.Scripts.Player.Controlls;
using Assets._Project.Scripts.Player.Glasses;
using Assets._Project.Scripts.Player.Interaction;
using UnityEngine;

namespace Assets._Project.Scripts.Player.States
{
    public class PlayerStateWalk : IPlayerState
    {
        private FirstPersonInputController _inputController;
        private PlayerController _playerController;
        private InteractionHandler _interactionHandler;
        private GlassesController _glasses;
        private bool _isPaused;

        public bool Paused
        {
            get => _isPaused;
            set
            {
                if (value)
                    Pause();
                else
                    Continue();

                _glasses.Paused = value;
            }
        }

        public PlayerState State => PlayerState.Walk;

        public PlayerStateWalk(PlayerController playerController, FirstPersonInputController inputController, 
            InteractionHandler interactionHandler, GlassesController glasses)
        {
            _playerController = playerController;
            _inputController = inputController;
            _interactionHandler = interactionHandler;
            _glasses = glasses;
        }

        public void Start()
        {
            _inputController.CharacterControllerActivity = true;
            _inputController.ExamineControllerActivity = false;
            _interactionHandler.Interactor = _glasses.IsReadyToUse ? _glasses : _playerController.EyesInteractor;
            Paused = _playerController.Paused;
        }

        private void Pause()
        {
            _inputController.Used -= OnUsed;
            _inputController.Switched -= OnSwitched;
            _isPaused = true;
        }

        public void Stop() => Pause();

        private void Continue()
        {
            _inputController.Used += OnUsed;
            _inputController.Switched += OnSwitched;
            _isPaused = false;
        }

        public void Dispose() => Stop();

        private void OnUsed()
        {
            if (_glasses.IsInMotion)
                return;

            if (_interactionHandler.TryUse(out IInteractable interactable))
            {
                _playerController.ChangeState(new PlayerStateExamine(_playerController, _inputController, interactable));
            }
        }

        private void OnSwitched()
        {
            if (_glasses.IsInMotion)
                return;

            _glasses.IsOn = !_glasses.IsOn;

            if (_glasses.IsOn)
            {
                _glasses.GlassesPrepared += OnGlassesPrepared;
            }
            else
            {
                _interactionHandler.Interactor = _playerController.EyesInteractor;
                _playerController.CheckPerception();
            }
        }

        private void OnGlassesPrepared()
        {
            _glasses.GlassesPrepared -= OnGlassesPrepared;
            _interactionHandler.Interactor = _glasses;
            _playerController.CheckPerception();
        }
    }
}
