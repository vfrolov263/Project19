using System.Collections;
using Assets._Project.Scripts.Gameplay.Characters;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Props
{
    public class PhoneInteractable : SimpleInteractable
    {
        [SerializeField]
        CharacterInteractable _dialogue;
        [SerializeField]
        private AudioSource _ringtone;

        public override bool Interact()
        {
            StopAllCoroutines();
            bool result = base.Interact();

            if (result)
                StartCoroutine(StartTalkRoutine());

            return result;
        }

        private IEnumerator StartTalkRoutine()
        {               
            yield return new WaitForFixedUpdate();
            yield return new WaitUntil(() => !base.IsInTransition);

            if (_ringtone != null)
            {
                _ringtone.Stop();
                _ringtone.enabled = false;
            }

            _dialogue.enabled = true;
            _dialogue.Interact();
        }

        protected override void OnExit()
        {
            _dialogue.enabled = false;
            StopAllCoroutines();
        }
    }
}