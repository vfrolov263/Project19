using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Rules.Progress
{
    [Serializable]
    public class ProgressRule
    {
        public string handlerName;
        public List<string> triggers;
    }

    [CreateAssetMenu(fileName = "ProgressConfig", menuName = "Scriptable Objects/ProgressConfig")]
    public class ProgressConfig : ScriptableObject, IEnumerable<ProgressRule>
    {
        [SerializeField]
        private List<ProgressRule> _rules;

        public IEnumerator<ProgressRule> GetEnumerator()
        {
            foreach (var rule in _rules)
                yield return rule;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}