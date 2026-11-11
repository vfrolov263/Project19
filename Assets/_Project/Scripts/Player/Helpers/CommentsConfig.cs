using System;
using System.Collections.Generic;
using System.Linq;
using Assets._Project.Scripts.Player;
using UnityEngine;
using UnityEngine.Localization;

namespace Assets._Project.Scripts.Player.Helpers
{
    [CreateAssetMenu(fileName = "CommentsConfig", menuName = "Scriptable Objects/CommentsConfig")]
    public class CommentsConfig : ScriptableObject
    {
        [Serializable]
        private class CommentSet
        {
            public string trigger;
            public LocalizedString comment;
        }

        [SerializeField]
        private List<CommentSet> _comments;

        public Dictionary<string, LocalizedString> GetCommentsDictionary() =>
            _comments != null ? _comments.ToDictionary(x => x.trigger, x => x.comment) : new();
    }
}