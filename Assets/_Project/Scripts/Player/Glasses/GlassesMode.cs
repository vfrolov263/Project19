using System;
using Assets._Project.Scripts.Gameplay.Pausable;
using Assets._Project.Scripts.Player.Interaction;
using UnityEngine;

namespace Assets._Project.Scripts.Player.Glasses
{
    public class GlassesController : IPausable, IInteractor
    {
        private Animator _animator;
        private bool _isOn;

        public event Action GlassesPrepared;

        public GlassesController(Animator animator, Camera camera)
        {
            _animator = animator;
            _interactor = new CameraInteractor(camera);
        }

        private IInteractor _interactor;

        public bool Paused 
        { 
            set
            {
                _animator.speed = value ? 0f : 1f;
            }
        }

        public bool IsOn
        {
            get => _isOn;
            set
            {
                _isOn = value;

                if (!_isOn)
                    IsReadyToUse = false;

                _animator.SetBool("isOn", _isOn);
            }
        }

        public bool IsReadyToUse { get; private set; }

        public Ray Ray => _interactor.Ray;
        
        public void ReadyToUse()
        {
            IsReadyToUse = true;
            GlassesPrepared?.Invoke();
        }
    }
}