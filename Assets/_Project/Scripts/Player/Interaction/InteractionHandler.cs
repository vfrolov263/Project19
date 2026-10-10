using System;
using System.Threading;
using UnityEngine;

namespace Assets._Project.Scripts.Player.Interaction
{
    public class InteractionHandler : IDisposable
    {
        public event Action<IInteractable> Selected;
        public event Action Deselected;

        public IInteractor Interactor
        {
            get => _interactor;
            set
            {
                if (value != null)
                    _interactor = value;
                else
                    Debug.LogWarning("Try set null interactor.");
            }
        }

        private IInteractor _interactor;
        private LayerMask _interactionLayer;
        private IInteractable _currentInteractable;
        private float _checkForInteractionAbilityDelay = Settings.Settings.CHECK_INTERACTION_INTERVAL;
        private CancellationTokenSource _cts;

        public InteractionHandler(IInteractor interactor = null, LayerMask interactionLayer = default)
        {
            _interactor = interactor ?? CameraInteractor.Default;
            _interactionLayer = (int)interactionLayer == 0 ? (LayerMask)(~0) : interactionLayer;
            _cts = new();
            _ = CheckForInteractionAbilityRoutine();
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }

        public bool TryUse()
        {
            if (FindInteractable(out IInteractable interactable))
            {
                interactable.Interact();
                return true;
            }

            return false;
        }

        public bool TryUse(out IInteractable interactable)
        {
            if (FindInteractable(out interactable))
            {
                interactable.Interact();
                return true;
            }

            return false;
        }

        private async Awaitable CheckForInteractionAbilityRoutine()
        {
            while (!_cts.IsCancellationRequested)
            {
                if (FindInteractable(out IInteractable interactable))
                {
                    if (_currentInteractable != interactable)
                    {
                        ResetInteractable();
                        _currentInteractable = interactable;
                        _currentInteractable.Select();
                        Selected?.Invoke(_currentInteractable);
                    }
                }
                else
                    ResetInteractable();

                await Awaitable.WaitForSecondsAsync(_checkForInteractionAbilityDelay, _cts.Token);
            }
        }

        private void ResetInteractable()
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.Deselect();
                _currentInteractable = null;
                Deselected?.Invoke();
            }
        }

        private bool FindInteractable(out IInteractable interactable)
        {
            interactable = null;
            //Debug.DrawRay(_interactor.Ray.origin, _interactor.Ray.direction, Color.red, 1f);
            return Physics.Raycast(_interactor.Ray, out var hit, 
                Settings.Settings.MAX_INTERACTION_DISTANCE, _interactor.Mask) &&
                hit.collider.TryGetComponent(out interactable) && 
                hit.distance <= interactable.InteractionDistance;
        }
    }
}