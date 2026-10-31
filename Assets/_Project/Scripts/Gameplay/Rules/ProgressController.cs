using System;
using System.Collections.Generic;
using System.Linq;
using Assets._Project.Scripts.Gameplay.Rules.TriggerHandler;
using Assets._Project.Scripts.Player;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Rules
{
    public class ProgressController : IDisposable
    {
        private PlayerController _playerController;
        private Dictionary<ITriggerHandler, HashSet<string>> _progressRules = new();
        private HashSet<string> _allTriggers = new();
        private HashSet<string> _collectedTriggers = new();
        private Dictionary<string, PlayerAction>  _triggersDictionary = new();

        public event Action<string> Progressed;

        public ProgressController(PlayerController playerController)
        {
            _playerController = playerController;
            _playerController.Actioned += OnPlayerActioned;
        }

        public void AddProgressRule(ITriggerHandler triggerHandler, string triggerName)
        {
            if (_progressRules.TryGetValue(triggerHandler, out var triggers))  
                triggers.Add(triggerName);
            else
                _progressRules.Add(triggerHandler, new() { triggerName });

            _allTriggers.Add(triggerName);
        }

        public void AddProgressRule(ITriggerHandler triggerHandler, List<string> triggerNames)
        {
            if (_progressRules.TryGetValue(triggerHandler, out var triggers))  
                triggers.Concat(triggerNames.ToHashSet());
            else
                _progressRules.Add(triggerHandler, triggerNames.ToHashSet());

            _allTriggers.UnionWith(triggerNames);
        }

        public void Dispose()
        {
            _playerController.Actioned -= OnPlayerActioned;
        }

        public void AddTriggersDictionary(string trigger, PlayerAction action)
        {
            _triggersDictionary.TryAdd(trigger, action);
        }

        public void AddTriggersDictionary(Dictionary<string, PlayerAction> triggersDictionary)
        {
            foreach (var trigger in triggersDictionary)
            {
                AddTriggersDictionary(trigger.Key, trigger.Value);
            }
        }

        private void OnPlayerActioned(PlayerActionMessage msg)
        {
            if (!_triggersDictionary.TryGetValue(msg.Interactable != null ? 
                msg.Interactable.Name : "non_interactable", out PlayerAction action))
                action = PlayerAction.SelectInteractable;

            if (IsEqualActions(msg.ActionType, action))//msg.ActionType == action )
            {
                if (_allTriggers.Contains(msg.Interactable.Name))
                {
                    _collectedTriggers.Add(msg.Interactable.Name);
                    CheckForTriggerDone();
                }
            }
        }

        private bool IsEqualActions(PlayerAction pAct, PlayerAction dAct) =>
            (pAct == dAct) || (pAct == PlayerAction.Interact && dAct == PlayerAction.SelectInteractable);

        private void CheckForTriggerDone()
        {
            List<ITriggerHandler> _comletedHandlers = new();

            foreach (var rule in _progressRules)
            {
                if (_collectedTriggers.IsSupersetOf(rule.Value))
                {
                    Debug.Log($"{rule.Key.Name} execute.");
                    rule.Key.Execute();
                    _comletedHandlers.Add(rule.Key);
                    Progressed?.Invoke(rule.Key.Name);
                }
            }

            foreach (var ruleKey in _comletedHandlers)
            {
                _progressRules.Remove(ruleKey);
                //_playerController.SayComment("Кажись телефон звонит...");
            }
        }
    }
}
