using System;
using Assets._Project.Scripts.Player.Interaction;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Props
{
    [RequireComponent(typeof(Collider))]
    public class InteractableSender : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private UnityEngine.Object _receiver;
        [SerializeField]
        private string _name;

        private IInteractable _interactableReceiver;

        public string Name => _name;
        public bool HasToExamine => _interactableReceiver.HasToExamine;
        public bool HasToInteract => _interactableReceiver.HasToInteract;
        public bool HasToDeepExamine => _interactableReceiver.HasToDeepExamine;
        public bool HasManualExit => _interactableReceiver.HasManualExit;
        public bool IsNeedPointer  => _interactableReceiver.IsNeedPointer;
        public event Action ExitRequested;
        public IInteractable Reciever => _interactableReceiver;

        private void Awake()
        {
            _interactableReceiver = _receiver as IInteractable;

            if (_interactableReceiver == null)
                throw new Exception($"Receiver {_receiver.name} is not IInteractable");
        }

        private void OnEnable() => _interactableReceiver.ExitRequested += OnExitRequested;
        private void OnDisable() => _interactableReceiver.ExitRequested -= OnExitRequested;
        public bool Interact() => _interactableReceiver.Interact();
        public void ExitInteraction(Action onDone = null) => _interactableReceiver.ExitInteraction(onDone);
        public void Select() => _interactableReceiver.Select();
        public void Deselect() => _interactableReceiver.Deselect();
        private void OnExitRequested() => ExitRequested?.Invoke();
    }
}