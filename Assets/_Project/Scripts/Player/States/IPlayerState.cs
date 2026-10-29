using System;
using Assets._Project.Scripts.Gameplay.Pausable;
using Assets._Project.Scripts.Player.Interaction;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets._Project.Scripts.Player.States
{
    public enum PlayerState
    {
        Walk,
        Examine,
        Door,
        Talk
    }

    public interface IPlayerState : IDisposable, IPausable
    {
        public PlayerState State { get; }
        public event Action<PlayerActionMessage> Actioned;
        void Start() {}
        void Stop() {}
        void OnUsed(IInteractable interactable = null) {}
        void OnExit() {}
        void OnSwitched() {}
    }
}