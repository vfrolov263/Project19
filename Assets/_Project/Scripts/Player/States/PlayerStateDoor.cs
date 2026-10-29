using System;
using Assets._Project.Scripts.Gameplay.Pausable;
using Assets._Project.Scripts.Gameplay.Props;
using Assets._Project.Scripts.Player.Controlls;
using Assets._Project.Scripts.Player.Interaction;
using UniRx;
using UnityEngine;
using Unity.Cinemachine;
using System.Threading;

namespace Assets._Project.Scripts.Player.States
{
    /// <summary>
    /// Режим открывания/закрытия двери.
    /// Движение только вперед/назад, камера позиционируется жестко,
    /// боковое движение и взгляд заблокированы.
    /// </summary>
    public class PlayerStateDoor : IPlayerState, IDisposable
    {
        private readonly PlayerController _playerController;
        private readonly FirstPersonInputController _inputController;
        private readonly DoorInteractable _doorInteractable;
        private readonly CinemachineCamera _doorCamera;
        private readonly CinemachineBrain _camBrain;

        private bool _isPaused;
        private CompositeDisposable _disposables;

        public PlayerState State => PlayerState.Door;

        public event Action<PlayerActionMessage> Actioned;

        public bool Paused
        {
            get => _isPaused;
            set
            {
                if (value)
                    Pause();
                else
                    Continue();

                _isPaused = value;
            }
        }

        public PlayerStateDoor(PlayerController playerController, FirstPersonInputController inputController,
            DoorInteractable doorInteractable)
        {
            _playerController = playerController;
            _inputController = inputController;
            _doorInteractable = doorInteractable;
        }

        public void Start()
        {
            if (!_doorInteractable.NeedManualControl)
            {
                _doorInteractable.ExitInteraction();
                _playerController.ChangeState(PlayerState.Walk);
                return;
            }

            _inputController.CharacterControllerActivity = false;
            _inputController.ExamineControllerActivity = true;
            Paused = _playerController.Paused;

            _disposables = new CompositeDisposable();
            CancellationTokenSource initialMovementCts = new();
            _disposables.Add(initialMovementCts);
            _ = InitialMovementRoutine(initialMovementCts.Token);
        }

        private async Awaitable InitialMovementRoutine(CancellationToken ct)
        {
            float initialMovementTime = 0f;
            float direction = 1f;//_doorInteractable.IsOpened ? -1f : 1f;

            while (initialMovementTime < Settings.Settings.PLAYER_INITIAL_DOOR_OPEN_MOVEMENT_TIME)
            {
                OnMoveInput(new(0f, direction));
                await Awaitable.NextFrameAsync(ct);
                initialMovementTime += Time.deltaTime;
            }

            _inputController.ExamineMove.Subscribe(OnMoveInput).AddTo(_disposables);
            _doorInteractable.Opened += OnOpened;
            _doorInteractable.Closed += OnClosed;
        }

        private void OnOpened()
        {
            _playerController.SetPosition(_doorInteractable.OpenerPosition.position,
                _doorInteractable.OpenerPosition.forward);
            _playerController.LookHorizontally();
            ExitToWalk();
        }

        private void OnClosed()
        {
            _playerController.SetPosition(_doorInteractable.CloserPosition.position,
                -_doorInteractable.CloserPosition.forward);
            ExitToWalk();
        }

        private void ExitToWalk()
        {
            _disposables?.Dispose();
            _disposables = null;
            _doorInteractable.ExitInteraction(() => _playerController.ChangeState(PlayerState.Walk));
        }

        private void OnMoveInput(Vector2 input)
        {
            float forward = input.y;
            if (Mathf.Abs(forward) < 0.01f)
                return;

            float force = forward * Time.deltaTime;
            _doorInteractable.Push(force);
        }

        public void Stop()
        {
            _disposables?.Dispose();
            _disposables = null;
            _doorInteractable.Opened -= OnOpened;
            _doorInteractable.Closed -= OnClosed;
            Pause();
        }

        private void Pause()
        {
            _isPaused = true;
        }

        private void Continue()
        {
            _isPaused = false;
        }

        public void Dispose() => Stop();

        public void OnUsed(IInteractable interactable = null) { }
        public void OnExit() { }
        public void OnSwitched() { }
    }
}