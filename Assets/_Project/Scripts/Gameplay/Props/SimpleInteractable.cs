using System;
using Assets._Project.Scripts.Player.Interaction;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;

namespace Assets._Project.Scripts.Gameplay.Props
{
    public class SimpleInteractable : MonoBehaviour, IInteractable
    {
        [field: SerializeField]
        public string Name { get; private set; }
        private CinemachineCamera _camera;
        private CinemachineBrain _camBrain;
        private Collider _collider;

        private void Awake()
        {
            _camera = GetComponentInChildren<CinemachineCamera>();
            _camBrain = Camera.main.GetComponent<CinemachineBrain>();
            _collider = GetComponent<Collider>();
            
            if (_camera == null)
                throw new ArgumentException($"{gameObject.name} vcam is null.");
        }

        public void Deselect()
        {
            Debug.Log($"{gameObject.name} deselected");
        }

        public void Interact()
        {
            Debug.Log($"{gameObject.name} interacted");

            // if (_camera.enabled)
            // {
            //     ExitInteraction();
            //     return;    
            // }

            _camera.enabled = true;
            _collider.enabled = false;
        }

        public void ExitInteraction(Action onDone = null) => _ = ExitRoutine(onDone);
        
        public void Select()
        {
            Debug.Log($"{gameObject.name} selected");
        }

        private async Awaitable ExitRoutine(Action onDone)
        {
            _camera.enabled = false;
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