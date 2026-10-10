using System;
using Assets._Project.Scripts.Gameplay.Animation;
using Assets._Project.Scripts.Gameplay.Pausable;
using Assets._Project.Scripts.Player.Interaction;
using UnityEngine;

namespace Assets._Project.Scripts.Player.Glasses
{
    public class GlassesController : IPausable, IInteractor, IDisposable
    {
        private Animator _animator;
        private bool _isOn;
        private AnimationEventsHandler _animationEventsHandler;
        private readonly string _readyToUseEventName, _motionStartEventName, _motionEndEventName;

        public event Action GlassesPrepared;

        public GlassesController(Animator animator, IInteractor interactor, AnimationEventsHandler animationEventsHandler,
            string readyToUseEventName, string motionStartEventName, string motionEndEventName)
        {
            _animator = animator;
            _interactor = interactor;
            _animationEventsHandler = animationEventsHandler;
            _readyToUseEventName = readyToUseEventName;
            _motionStartEventName = motionStartEventName;
            _motionEndEventName = motionEndEventName;
            _animationEventsHandler.OnEvent += OnAnimationEvent;
        }

        public void Dispose()
        {
            _animationEventsHandler.OnEvent -= OnAnimationEvent;
        }
        
        private IInteractor _interactor;

        public bool Paused 
        { 
            get => Mathf.Approximately(_animator.speed, 1f);
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
                IsInMotion = true;
            }
        }

        public bool IsReadyToUse { get; private set; }

        public bool IsInMotion { get; private set; }

        public Ray Ray => _interactor.Ray;

        public LayerMask Mask => _interactor.Mask;

        public void ReadyToUse()
        {
            IsInMotion = false;
            IsReadyToUse = true;
            GlassesPrepared?.Invoke();
        }

        public void ReadyToHide()
        {
            IsInMotion = false;
        }

        private void OnAnimationEvent(string name)
        {
            if (name == _readyToUseEventName)
            {
                IsReadyToUse = _isOn;

                if (IsReadyToUse)
                    GlassesPrepared?.Invoke();
            }
            else if (name == _motionStartEventName)
                IsInMotion = _isOn;
            else if (name == _motionEndEventName)
                IsInMotion = !_isOn;
            
        }
    }
}