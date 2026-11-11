using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Props
{
    public class LockInteractable : SimpleInteractable
    {
        [SerializeField]
        List<LockWheelInteractable> _wheels;

        [SerializeField]
        private GameObject _pipe;

        [SerializeField]
        private DoorController _door;

        [SerializeField]
        private Collider _handle;

        [SerializeField]
        private AudioSource _unlockSound;

        public override event Action ExitRequested;

        public override bool HasToDeepExamine => true;

        private void OnEnable()
        {
            foreach (var wheel in _wheels)
                wheel.Changed += OnChanged;
        }

        private void OnDisable()
        {
            foreach (var wheel in _wheels)
                wheel.Changed -= OnChanged;
        }

        private void OnChanged(LockWheelInteractable _)
        {
            if (_wheels.Count(x => x.IsCorrectPosition) != _wheels.Count)
                return;

            foreach (var wheel in _wheels)
            {
                Destroy(wheel.GetComponent<Collider>());
            }

            ExitRequested?.Invoke();
            _door.IsLocked = false;
            _door.gameObject.SetActive(true);
            _handle.enabled = true;

            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.linearDamping = 10f;
            rb = _pipe.AddComponent<Rigidbody>();
            rb.linearDamping = 10f;
            Destroy(_pipe, 4f);
            Destroy(gameObject, 4f);

            // For playtest only
           /* _unlockSound.Play();

            ExitRequested?.Invoke();
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.linearDamping = 10f;
            rb = _pipe.AddComponent<Rigidbody>();
            rb.linearDamping = 10f;
            Destroy(_pipe, 4f);
            Destroy(gameObject, 4f);

            rb = _door.AddComponent<Rigidbody>();
            rb.linearDamping = 5f;
            rb.angularDamping = 5f;
            rb.AddForce(new(0f, 0f, 100f));

            // Settings.Settings.ReloadGame(7f);
            FinalStarter finalStarter = GameObject.FindFirstObjectByType<FinalStarter>();

            if (finalStarter != null)
                finalStarter.StartFinal();*/
        }
    }
}
