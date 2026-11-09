using System;
using System.Collections;
using Assets._Project.Scripts.Player.Controlls;
using Assets._Project.Scripts.Player.Interaction;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Props
{
    public class DoubleViewInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private UnityEngine.Object _interactableObj;
        [SerializeField]
        private CinemachineCamera _camera;
        [SerializeField]
        private Collider _collider;

        private IInteractable _interactable;

        private bool _isSecondIntraction, _isRedayToSwitch;

        [field: SerializeField]
        public string Id { get; private set; }
        public string Name => _interactable.Name;

        public bool HasToExamine => _interactable.HasToExamine;

        public bool HasToInteract => _interactable.HasToInteract;

        public float InteractionDistance => _interactable.InteractionDistance;

        public bool IsNeedPointer => _interactable.IsNeedPointer;

        public event Action ExitRequested;

        private  CinemachineBrain _camBrain;

        private FirstPersonInputController _inputController;


        private void Awake()
        {
            if (_interactableObj == null)
                throw new ArgumentException($"{gameObject.name} interactable is null.");

            _interactable = _interactableObj as IInteractable;

            if (_interactable == null)
                throw new ArgumentException($"{gameObject.name} interactable is not IInteractable.");

            if (_camera == null)
                _camera = GetComponentInChildren<CinemachineCamera>();

            _camBrain = Camera.main.GetComponent<CinemachineBrain>();
            _inputController = GameObject.FindFirstObjectByType<FirstPersonInputController>();
        }
        

        public bool Interact()
        {
            _isRedayToSwitch = false;
            StopAllCoroutines();

            bool result = _interactable.Interact();

            if (result)
            {
                StartCoroutine(StartInteractRoutine());

                if (_collider != null)
                    _collider.enabled = false;
            }

            return result;
        }

        private IEnumerator StartInteractRoutine()
        {               
            yield return new WaitForFixedUpdate();
            yield return new WaitUntil(() => _camBrain == null || !_camBrain.IsBlending);
            _isRedayToSwitch = true;
        }

        public void ExitInteraction(Action onDone = null)
        {
            if (!_isRedayToSwitch)
                return;
            
            _isRedayToSwitch = false;
            _interactable.ExitInteraction(() => 
            {
                _isRedayToSwitch = true;
                onDone();
                
                if (_isSecondIntraction)
                {
                    _isSecondIntraction = false;
                    return;
                }

                _isSecondIntraction = true;
                _inputController.BotCmdSwitch();
                StartCoroutine(KnockToInteractRoutine());
                //Interact();
            });

            _camera.enabled = !_isSecondIntraction;
            
            if (_collider != null)
                _collider.enabled = true;
        }

        private IEnumerator KnockToInteractRoutine()
        {
            while (true)
            {
                _inputController.BotCmdUse();
                yield return null;
            }
        }
    }
}