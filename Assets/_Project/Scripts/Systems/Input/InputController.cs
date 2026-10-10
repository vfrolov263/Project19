using UnityEngine;

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
        
    }
}
