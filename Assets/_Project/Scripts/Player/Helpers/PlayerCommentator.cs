using System;
using UnityEngine;
using Assets._Project.Scripts.Gameplay.Rules;
using System.Collections.Generic;

namespace Assets._Project.Scripts.Player.Helpers
{
    public class PlayerCommentator : IDisposable
    {
        private PlayerController _playerController;
        private ProgressController _progressController;

        private Dictionary<string, string> _comments;

        public PlayerCommentator(PlayerController playerController, ProgressController progressController,
            Dictionary<string, string> comments)
        {
            _playerController = playerController;
            _progressController = progressController;
            _comments = comments;
            _playerController.Actioned += OnActioned;
            _playerController.Disposed += Dispose;
            _progressController.Progressed += OnProgressed;

        }

        private void OnActioned(PlayerActionMessage msg)
        {
            
        }

        private void OnProgressed(string trigger)
        {
            if (_comments.TryGetValue(trigger, out string comment))
            {
                _playerController.SayComment(comment);   
            }
        }

        public void Dispose()
        {
            _playerController.Disposed -= Dispose;
            _playerController.Actioned -= OnActioned;
            _progressController.Progressed -= OnProgressed;
        }
    }
}