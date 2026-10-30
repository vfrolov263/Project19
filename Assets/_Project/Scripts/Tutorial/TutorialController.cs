using System;
using UnityEngine;
using Assets._Project.Scripts.Gameplay.Rules;
using System.Collections.Generic;
using Assets._Project.Scripts.Player;
using System.Linq;

namespace Assets._Project.Scripts.Tutorial
{
    public enum ClueType
    {
        Interact,
        ExitInteract,
        UseGlasses,
        Unknown
    }

    public class TutorialController : IDisposable
    {
        private PlayerController _playerController;
        private ProgressController _progressController;
        private TutorialView _view;

        private Dictionary<ClueType, int> _clues;

        private bool _isClueShown;

        private Func<PlayerActionMessage, bool> _hideCondition;

        public TutorialController(PlayerController playerController, ProgressController progressController, TutorialView view)
        {
            _playerController = playerController;
            _progressController = progressController;
            _view = view;
            _playerController.Actioned += OnActioned;
            _playerController.Disposed += Dispose;
            _progressController.Progressed += OnProgressed;
            
            _clues = new()
            {
                {ClueType.Interact, -1},
                {ClueType.ExitInteract, -1},
                {ClueType.UseGlasses, 1}
            };

            _hideCondition = IsAnyConditionSatisfied;
        }

        public void Dispose()
        {
            _playerController.Disposed -= Dispose;
            _playerController.Actioned -= OnActioned;
            _progressController.Progressed -= OnProgressed;

            if (_view != null && _view.gameObject != null)
                GameObject.Destroy(_view.gameObject);
        }

        private void OnActioned(PlayerActionMessage msg)
        {
            if (_hideCondition != null && !_hideCondition(msg))
                return;
            
            HideClue();
            bool isInteractable = msg.Interactable != null && msg.Interactable.HasToInteract;

            if (!isInteractable)
                return;

            if (msg.ActionType == PlayerAction.SelectInteractable)
            {
                ShowClue(ClueType.Interact);
            }
            else if (msg.ActionType == PlayerAction.Interact && msg.Interactable.HasToExamine)
            {
                ShowClue(ClueType.ExitInteract);
                _hideCondition = IsExitInteractClueHideConditionSatisfied;
            }
        }

        private void OnProgressed(string trigger)
        {
            if (trigger == "get_glasses")
            {
                _hideCondition = IsUseGlassesHideConditionSatisfied;
                _view.HideClue();
                ShowClue(ClueType.UseGlasses, true);
                _progressController.Progressed -= OnProgressed;
            }
        }

        private void ResetClues()
        {
            List<ClueType> cluesToRemove = _clues.Where(x => x.Value == 0).Select(x => x.Key).ToList();

            foreach (ClueType type in cluesToRemove)
                _clues.Remove(type);

            if (_clues.Count == 0)
                Dispose();
        }

        private void ShowClue(ClueType type, bool forced = false)
        {
            if (!forced && _isClueShown)
                return;

            if (!_clues.ContainsKey(type))
                return;

            if (_clues[type] > 0)
                _clues[type]--;

            //Debug.Log($"Clue of type {type}");
            _view.ShowClue(type);
            _isClueShown = true;
        }

        private void HideClue()
        {
            if (_isClueShown)
            {
                //Debug.Log("Hide clue");
                _hideCondition = IsAnyConditionSatisfied;
                _view.HideClue();
                _isClueShown = false;
                ResetClues();
            }
        }

        private bool IsAnyConditionSatisfied(PlayerActionMessage _) => true;

        private bool IsExitInteractClueHideConditionSatisfied(PlayerActionMessage msg)
        {
            return msg.ActionType == PlayerAction.ExitInteract;
        }

        private bool IsUseGlassesHideConditionSatisfied(PlayerActionMessage msg)
        {
            return msg.ActionType == PlayerAction.PutOnGlasses;
        }
    }
}