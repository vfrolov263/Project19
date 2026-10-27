using System;
using Assets._Project.Scripts.Gameplay.Props;
using UnityEngine;
using Yarn.Unity;

namespace Assets._Project.Scripts.Gameplay.Characters
{
    public class CharacterInteractable : SimpleInteractable
    {
        [SerializeField]
        private string _dialogueName;
        private DialogueRunner _dialogueRunner;

        public event Action DialogueEnded;

        protected override void Awake()
        {
            base.Awake();
            _dialogueRunner = FindFirstObjectByType<DialogueRunner>();
        }

        protected void OnEnable()
        {
            _dialogueRunner.onDialogueStart.AddListener(OnDialogueStarted);
            _dialogueRunner.onDialogueComplete.AddListener(OnDialogueEnded);
        }

        protected void OnDisable()
        {
            _dialogueRunner.Stop();
            _dialogueRunner.onDialogueStart.RemoveListener(OnDialogueStarted);
            _dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueEnded);
        }

        public override bool Interact()
        {
            bool result = base.Interact();

            if (result)
                StartDialogue();

            return result;
        }

        protected void StartDialogue()
        {
            _dialogueRunner.StartDialogue(_dialogueName);
        }

        private void OnDialogueStarted()
        {

        }

        private void OnDialogueEnded()
        {
            DialogueEnded?.Invoke();    
        }
    }
}