using System;
using Assets._Project.Scripts.Player;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Rules.TriggerHandler
{
    public class GetGlassesTrigger : ITriggerHandler
    {
        private PlayerController _playerController;

        public GetGlassesTrigger(PlayerController playerController)
        {
            _playerController = playerController;
        }

        public string Name => "get_glasses";

        public void Execute()
        {
            _playerController.HasGlasses = true;
        }
    }
}