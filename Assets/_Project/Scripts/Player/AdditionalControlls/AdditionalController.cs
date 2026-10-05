using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets._Project.Scripts.Player.AdditionalControlls
{
    public class AdditionalController : MonoBehaviour
    {
        public event Action Used, Switched;

        private AdditionalInputController _controls;


        private void Awake()
        {
            _controls = new();
        }

        private void OnEnable()
        {
            _controls.Enable();
            _controls.Additional.Use.performed += OnUsed;
            _controls.Additional.Switch.performed += OnSwitched;
        }

        private void OnDisable()
        {
            _controls.Disable();
            _controls.Additional.Use.performed -= OnUsed;
            _controls.Additional.Switch.performed -= OnSwitched;
        }

        private void OnUsed(InputAction.CallbackContext _) => Used?.Invoke();

        private void OnSwitched(InputAction.CallbackContext _) => Switched?.Invoke();
    }
}