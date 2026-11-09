using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets._Project.Scripts.Gameplay.Characters;
using Assets._Project.Scripts.Gameplay.Rules.EventsCascade;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Props
{
    public class PhoneInteractable : SimpleInteractable, IEvent
    {
        [SerializeField]
        private List<CharacterInteractable> _dialogues;
        [SerializeField]
        private AudioSource _ringtone;
        [SerializeField]
        private bool _isRemovableDialogues = true;

        public event Action<IEvent> Started, Finished, Interrupted;
        public override event Action ExitRequested;

        private CharacterInteractable _dialogue;

        public override bool HasManualExit => false;

        public override bool Interact()
        {
            if (_dialogues.Count == 0)
                return false;

            _dialogue = _dialogues.First();

            StopAllCoroutines();
            bool result = base.Interact();

            if (result)
                StartCoroutine(StartTalkRoutine());

            return result;
        }

        public override void ExitInteraction(Action onDone = null)
        {
            if (!_isRemovableDialogues || _dialogues.Count == 0)
                base.ExitInteraction(onDone);
        }

        public void AddDialogue(CharacterInteractable dialogue)
        {
            if (_dialogue != null)
                _dialogues.Add(dialogue);
        }

        void IEvent.Start()
        {
            Interact();
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
            _dialogue.DialogueEnded += OnDialogueEnded;
            _dialogue.Interact();
        }

        protected override void OnExit()
        {
            if (_dialogue != null)
            {
                _dialogue.DialogueEnded -= OnDialogueEnded;
                _dialogue.enabled = false;
            }

            StopAllCoroutines();
        }

        private void OnDialogueEnded()
        {
            _dialogue.DialogueEnded -= OnDialogueEnded;

            if (_isRemovableDialogues)
                _dialogues.RemoveAt(0);

            Finished?.Invoke(this);

            if (!_isRemovableDialogues || _dialogues.Count == 0)
                ExitRequested?.Invoke();
        }
    }
}