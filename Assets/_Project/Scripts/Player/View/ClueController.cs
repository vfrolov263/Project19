using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Scripts.Player.View
{
    public class ClueController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _clue;
        [SerializeField]
        private Image _pointer;
        [SerializeField]
        private float _pointerFocusedScale;
        [SerializeField]
        private float _timeToClue;
        [SerializeField]
        private TMP_Text _comment;

        private WaitForSeconds _clueDelay;
        private Coroutine _clueRoutine, _commentRoutine;

        private void Awake()
        {
            _clueDelay = new(_timeToClue);
        }

        public void ShowClue(string clueText)
        {
            HideClue();
            _pointer.rectTransform.localScale = new(_pointerFocusedScale, _pointerFocusedScale);
            _clueRoutine = StartCoroutine(ShowClueRoutine(clueText));
        }

        public void HideClue()
        {
            _pointer.rectTransform.localScale = new(1f, 1f);
            
            if (_clueRoutine != null)
                StopCoroutine(_clueRoutine);
            
            _clue.text = "";
        }

        public void SayComment(string comment)
        {
            HideComment();
            _comment.text = comment;
            _commentRoutine = StartCoroutine(HideCommentRoutine());
        }

        public void HideComment()
        {
            if (_commentRoutine != null)
                StopCoroutine(_commentRoutine);

            _comment.text = "";
        }

        private IEnumerator ShowClueRoutine(string clueText)
        {
            yield return _clueDelay;
            _clue.text = clueText;
        }

        private IEnumerator HideCommentRoutine()
        {
            int extraLenght = _comment.text.Length - Settings.Settings.COMMENT_BASE_LENGTH;
            
            if (extraLenght < 0)
                extraLenght = 0;

            float time = Settings.Settings.COMMENT_BASE_TIME + extraLenght * Settings.Settings.COMMENT_TIME_PER_EXTRA_LETTER;
            yield return new WaitForSeconds(time);
            _comment.text = "";
        }
    }
}
