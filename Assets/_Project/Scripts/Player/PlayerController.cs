using System;
using Assets._Project.Scripts.Gameplay.Pausable;
using Assets._Project.Scripts.Player.AdditionalControlls;
using Assets._Project.Scripts.Player.Glasses;
using Assets._Project.Scripts.Player.Interaction;
using DyrdaDev.FirstPersonController;
using UnityEngine;

namespace Assets._Project.Scripts.Player
{
    public class PlayerController : IPausable, IDisposable
    {
        public event Action<bool> WorldSwitched;
        private readonly InteractionHandler _interactionHandler;
        private readonly FirstPersonController _fpController;
        private readonly AdditionalController _addController;
        private GlassesController _glasses;

        public PlayerController(FirstPersonController fpController, AdditionalController addController,
            InteractionHandler interactionHandler, GlassesController glasses)
        {
            _fpController = fpController;
            _addController = addController;
            _addController.Used += OnUsed;
            _addController.Switched += OnSwitched;
            _interactionHandler = interactionHandler;
            _glasses = glasses;
        }

        public bool Paused 
        { 
            set
            {
                _fpController.enabled = value;
                _addController.enabled = value;
            }
        }

        public void Dispose()
        {
            _interactionHandler.Dispose();
            _addController.Used -= OnUsed;
            _addController.Switched -= OnSwitched;
        }

        private void OnUsed()
        {
            _interactionHandler.TryUse();
        }

        private void OnSwitched()
        {
            _glasses.IsOn = !_glasses.IsOn;

            if (_glasses.IsOn)
            {
                _glasses.GlassesPrepared += OnGlassesPrepared;
            }

            WorldSwitched?.Invoke(_glasses.IsOn);
        }

        private void OnGlassesPrepared()
        {
            _glasses.GlassesPrepared -= OnGlassesPrepared;
            _interactionHandler.Interactor = _glasses;
        }
    }
}