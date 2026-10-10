using System;
using Assets._Project.Scripts.Gameplay.Pausable;
using Assets._Project.Scripts.Player.Controlls;
using Assets._Project.Scripts.Player.Glasses;
using Assets._Project.Scripts.Player.Interaction;
using Assets._Project.Scripts.Player.States;
using Assets._Project.Scripts.Player.View;
using Unity.Cinemachine;
using UnityEngine;
using VContainer.Unity;

namespace Assets._Project.Scripts.Player
{
    public class PlayerController : IPausable, IDisposable, IStartable
    {
        public event Action<bool> WorldSwitched;
        public IInteractor EyesInteractor { get; private set; }
        private readonly InteractionHandler _interactionHandler;
        // private readonly FirstPersonController _fpController;
        private GlassesController _glasses;
        private FirstPersonInputController _inputController;
        private IPlayerState _currentState;
        private CinemachineCamera _defaultCamera;
        private ClueController _clueController;

        // public PlayerController(FirstPersonController fpController, FirstPersonInputController addController,
        //     InteractionHandler interactionHandler, GlassesController glasses)
        // {
        //     _fpController = fpController;
        //     _addController = addController;
        //     _addController.Used += OnUsed;
        //     _addController.Switched += OnSwitched;
        //     _interactionHandler = interactionHandler;
        //     _glasses = glasses;
        // }

        public PlayerController(FirstPersonInputController inputController, InteractionHandler interactionHandler,
            GlassesController glasses, CinemachineCamera defaultCamera, ClueController clueController)
        {
            _inputController = inputController;
            _interactionHandler = interactionHandler;
            _glasses = glasses;
            EyesInteractor = new CameraInteractor(Camera.main, mask: LayerMask.GetMask("RealWorld"), rayOffset: new(.5f, .5f));
            _defaultCamera = defaultCamera;
            ChangeState(PlayerState.Walk);
            _clueController = clueController;
            _interactionHandler.Selected += OnInteractableSelected;
            _interactionHandler.Deselected += OnInteractableDeselected;
        }

        public bool Paused 
        {
            get => _currentState != null ? _currentState.Paused : false;
            set 
            { 
                if (_currentState != null) 
                    _currentState.Paused = value;
            }

        }

        public void Dispose()
        {
            _interactionHandler.Selected -= OnInteractableSelected;
            _interactionHandler.Deselected -= OnInteractableDeselected;
            _currentState.Dispose();
            _interactionHandler.Dispose();
            // _addController.Used -= OnUsed;
            // _addController.Switched -= OnSwitched;
        }

        public void ChangeState(PlayerState state)
        {
            if (_currentState != null && _currentState.State == state)
                return;

            _currentState?.Dispose();

            _currentState = state switch
            {
                PlayerState.Walk => new PlayerStateWalk(this, _inputController, _interactionHandler, _glasses),
                PlayerState.Examine => new PlayerStateExamine(this, _inputController, null),// new PlayerState(this)
                _ => throw new ArgumentException("Unknown player state"),
            };

            _currentState?.Start();
        }

        /// <remarks>
        /// Use with caution.
        /// </remarks>
        /// <exception cref="ArgumentException"></exception>
        public void ChangeState(IPlayerState state)
        {
            if (state == null)
                throw new ArgumentException("new player state is null.");

            if (_currentState != null && _currentState.State == state.State)
                return;

            _currentState?.Dispose();
            _currentState = state;
            _currentState.Start();
        }


        public void CheckPerception()
        {
            WorldSwitched?.Invoke(_glasses.IsOn);
        }

        public void Start()
        {
            _defaultCamera.enabled = false;
            _defaultCamera.enabled = true;
        }

        private void OnInteractableSelected(IInteractable interactable)
        {
            _clueController.ShowClue(interactable.Name);
        }

        private void OnInteractableDeselected()
        {
            _clueController.HideClue();
        }


        // private void OnUsed()
        // {
        //     _interactionHandler.TryUse();
        // }

        // private void OnSwitched()
        // {
        //     _glasses.IsOn = !_glasses.IsOn;

        //     if (_glasses.IsOn)
        //     {
        //         _glasses.GlassesPrepared += OnGlassesPrepared;
        //     }

        //     WorldSwitched?.Invoke(_glasses.IsOn);
        // }

        // private void OnGlassesPrepared()
        // {
        //     _glasses.GlassesPrepared -= OnGlassesPrepared;
        //     _interactionHandler.Interactor = _glasses;
        // }
    }
}