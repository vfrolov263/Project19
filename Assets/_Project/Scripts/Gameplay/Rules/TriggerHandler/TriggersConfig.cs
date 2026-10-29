using System;
using System.Collections.Generic;
using System.Linq;
using Assets._Project.Scripts.Player;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Rules.TriggerHandler
{
    [CreateAssetMenu(fileName = "TriggersConfig", menuName = "Scriptable Objects/TriggersConfig")]
    public class TriggersConfig : ScriptableObject
    {
        [Serializable]
        private struct TriggerSet
        {
            public string name;
            public PlayerAction action;
        }

        [SerializeField]
        private List<TriggerSet> _triggers;

        public Dictionary<string, PlayerAction> GetTriggersDictionary() => 
            _triggers != null ? _triggers.ToDictionary(x => x.name, x => x.action) : new();
    }
}