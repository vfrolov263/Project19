using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Scripts.Tutorial
{
    public class TutorialView : MonoBehaviour
    {
        [Serializable]
        private class ClueOption
        {
            public ClueType type;
            public Sprite sprite;
            public string text;
        }

        [SerializeField]
        private TMP_Text _clueText;
        [SerializeField]
        private Image _image;

        [SerializeField]
        private List<ClueOption> _clues;

        private Dictionary<ClueType, ClueOption> _cluesDict;

        private void Awake()
        {
            _cluesDict = new();

            foreach (var clue in _clues)
                _cluesDict.TryAdd(clue.type, clue);
        } 

        public void ShowClue(ClueType type)
        {
            if (_cluesDict.TryGetValue(type, out ClueOption option))
            {
                _image.sprite = option.sprite;
                
                if (_image.sprite != null)
                    _image.color = new(1f, 1f, 1f, 1f);

                _clueText.text = option.text;
            }
        }

        public void HideClue()
        {
            _clueText.text = "";
            _image.sprite = null;
            _image.color = new(1f, 1f, 1f, 0f);
        }
    }
}