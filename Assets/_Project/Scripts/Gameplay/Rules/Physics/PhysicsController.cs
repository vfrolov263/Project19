using System;
using Assets._Project.Scripts.Player;
using UnityEngine;
using VContainer.Unity;

namespace Assets._Project.Scripts.Gameplay.Rules.Physics
{
    public class PhysicsController : IStartable, IDisposable
    {
        [SerializeField]
        private int _playerLayerId, _realWorldLayerId, _alterWorldLayerId;
        private PlayerController _playerController;

        public PhysicsController(PlayerController playerController, int playerLayerId,
            int realWorldLayerId, int alterWorldLayerId)
        {
            _playerController = playerController;
            _playerLayerId = playerLayerId;
            _realWorldLayerId = realWorldLayerId;
            _alterWorldLayerId = alterWorldLayerId;
        }

        public void Start()
        {
            _playerController.WorldSwitched += OnWorldSwitched;
            UnityEngine.Physics.IgnoreLayerCollision(_playerLayerId, _realWorldLayerId, true);
        }

        public void Dispose()
        {
            _playerController.WorldSwitched -= OnWorldSwitched;
        }

        private void OnWorldSwitched(bool isAlterWorld)
        {
            UnityEngine.Physics.IgnoreLayerCollision(_playerLayerId, _realWorldLayerId, isAlterWorld);
            UnityEngine.Physics.IgnoreLayerCollision(_playerLayerId, _alterWorldLayerId, !isAlterWorld);
        }
    }
}