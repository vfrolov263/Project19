using System;
using Assets._Project.Scripts.Systems.Input;
using DyrdaDev.FirstPersonController;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets._Project.Scripts.Player.Controlls
{
    public class FirstPersonInputController : FirstPersonControllerInput
    {
        #region Controller Input Fields

        public override IObservable<Vector2> Move => _move;
        private IObservable<Vector2> _move;

        public override IObservable<Unit> Jump => _jump;
        private Subject<Unit> _jump;

        public override ReadOnlyReactiveProperty<bool> Run => _run;
        private ReadOnlyReactiveProperty<bool> _run;

        public override IObservable<Vector2> Look => _look;
        private IObservable<Vector2> _look;

        public event Action Used, Switched, Examined, Exited;

        #endregion

        #region Configuration

        [Header("Look Properties")]
        [SerializeField] private float lookSmoothingFactor = 14.0f;

        private Systems.Input.PlayerInput _controls;

        #endregion

        private void OnEnable()
        {
            _controls.Enable();
            _controls.Character.Use.performed += OnUsed;
            _controls.Character.Switch.performed += OnSwitched;
            _controls.Examine.Use.performed += OnExamined;
            _controls.Examine.Exit.performed += OnExited;
        }

        private void OnDisable()
        {
            _controls.Character.Use.performed -= OnUsed;
            _controls.Character.Switch.performed -= OnSwitched;
            _controls.Examine.Use.performed -= OnExamined;
            _controls.Examine.Exit.performed -= OnExited;
            _controls.Disable();
        }

        protected void Awake()
        {
            _controls = new Systems.Input.PlayerInput();

            // Hide the mouse cursor and lock it in the game window.
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Move:
            _move = this.UpdateAsObservable()
                .Select(_ => _controls.Character.Move.ReadValue<Vector2>());

            // Jump:
            _jump = new Subject<Unit>().AddTo(this);
            _controls.Character.Jump.performed += context => _jump.OnNext(Unit.Default);


            // Run:
            _run = this.UpdateAsObservable()
                .Select(_ => _controls.Character.Run.ReadValueAsObject() != null)
                .ToReadOnlyReactiveProperty();

            // Look:
            var smoothLookValue = new Vector2(0, 0);
            _look = this.UpdateAsObservable()
                .Select(_ =>
                {
                    var rawLookValue = _controls.Character.Look.ReadValue<Vector2>();

                    smoothLookValue = new Vector2(
                        Mathf.Lerp(smoothLookValue.x, rawLookValue.x, lookSmoothingFactor * Time.deltaTime),
                        Mathf.Lerp(smoothLookValue.y, rawLookValue.y, lookSmoothingFactor * Time.deltaTime)
                    );

                    return smoothLookValue;
                });
        }

        private void OnUsed(InputAction.CallbackContext _) => Used?.Invoke();
        private void OnSwitched(InputAction.CallbackContext _) => Switched?.Invoke();
        private void OnExamined(InputAction.CallbackContext _) => Examined?.Invoke();
        private void OnExited(InputAction.CallbackContext _) => Exited?.Invoke();

        public bool CharacterControllerActivity 
        { 
            get => _controls.Character.enabled;
            set
            {
                if (value)
                    _controls.Character.Enable();
                else
                    _controls.Character.Disable();
            }
        }

        public bool ExamineControllerActivity 
        { 
            get => _controls.Examine.enabled;
            set
            {
                if (value)
                    _controls.Examine.Enable();
                else
                    _controls.Examine.Disable();
            }
        }
    }
}