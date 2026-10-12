using Assets._Project.Scripts.Gameplay.Characters;
using Assets._Project.Scripts.Player.Controlls;
using Assets._Project.Scripts.Player.Interaction;

namespace Assets._Project.Scripts.Player.States
{
    public class PlayerStateTalk : PlayerStateExamine
    {
        public override PlayerState State => PlayerState.Talk;

        private CharacterInteractable _characterInteractable;

        public PlayerStateTalk(PlayerController playerController, FirstPersonInputController inputController,
            CharacterInteractable interactable) : base(playerController, inputController, interactable)
        {
            _characterInteractable = interactable;
        }     

        protected override void Pause()
        {
            base.Pause();
            _characterInteractable.DialogueEnded -= OnDialogueEnded;
        }

        protected override void Continue()
        {
            base.Continue();
            _characterInteractable.DialogueEnded += OnDialogueEnded;
        }

        private void OnDialogueEnded()
        {
            _characterInteractable.ExitInteraction(() => _playerController.ChangeState(PlayerState.Walk));
        }

        private void OnExamined()
        { 
        }

        sealed protected override void OnExited()
        {
        }
    }
}
