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

        private WaitForSeconds _clueDelay;

        private void Awake()
        {
            _clueDelay = new(_timeToClue);
        }

        public void ShowClue(string clueText)
        {
            HideClue();
            _pointer.rectTransform.localScale = new(_pointerFocusedScale, _pointerFocusedScale);
            StartCoroutine(ShowClueRoutine(clueText));
        }

        public void HideClue()
        {
            _pointer.rectTransform.localScale = new(1f, 1f);
            StopAllCoroutines();
            _clue.text = "";
        }

        private IEnumerator ShowClueRoutine(string clueText)
        {
            yield return _clueDelay;
            _clue.text = clueText;
        }
    }
}
