#define ESC_STRONG_EXIT
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets._Project.Scripts.Systems.Input
{
    public class InputController : MonoBehaviour
    {
        private bool _isFreedomCursor;

        public PlayerInput Input { get; private set; }

        private void Awake()
        {
            Input = new();
        }

        private void OnEnable()
        {
            Input.Enable();
        }

        private void OnDisable()
        {
            Input.Disable();
        }

        private bool IsFreedomCursor
        {
            get => _isFreedomCursor;
        }

#if ESC_STRONG_EXIT
        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Debug.Log("Exit");
                Application.Quit();
            }
        }   
#endif
    }
}
