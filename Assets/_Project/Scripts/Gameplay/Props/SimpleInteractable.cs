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
        [field: SerializeField]
        private bool HasToExamine = true;
        [field: SerializeField]
        public float InteractionDistance {get; private set; } = Settings.Settings.DEFAULT_INTERACTION_DISTANCE;
        [field: SerializeField]
        private CinemachineCamera _camera;
        private CinemachineBrain _camBrain;
        private Collider _collider;

        protected bool IsInTransition => _camBrain != null && _camBrain.IsBlending;

        protected virtual void Awake()
        {
            if (_camera == null)
                _camera = GetComponentInChildren<CinemachineCamera>();

            _camBrain = Camera.main.GetComponent<CinemachineBrain>();
            _collider = GetComponent<Collider>();
            
            // if (_camera == null)
            //     throw new ArgumentException($"{gameObject.name} vcam is null.");
        }

        public void Deselect()
        {
            Debug.Log($"{gameObject.name} deselected");
        }

        public virtual bool Interact()
        {
#if TEST_RUN
            Debug.Log($"{gameObject.name} interacted");
#endif
            if (!HasToExamine)
                return false;

            if (_camera != null)
                _camera.enabled = true;

            if (_collider != null)
                _collider.enabled = false;

            return true;
        }

        public void ExitInteraction(Action onDone = null) => _ = ExitRoutine(onDone);
        
        public void Select()
        {
            Debug.Log($"{gameObject.name} selected");
        }

        private async Awaitable ExitRoutine(Action onDone)
        {
            OnExit();

            if (_camera != null)
                _camera.enabled = false;
                
            await Awaitable.NextFrameAsync(destroyCancellationToken);

            while (IsInTransition)
            {
                await Awaitable.FixedUpdateAsync(destroyCancellationToken);
            }

            if (_collider != null)
                _collider.enabled = true;
                
            onDone?.Invoke();
        }

        protected virtual void OnExit() {}
    }
}