using System;

namespace Assets._Project.Scripts.Player.Interaction
{
    public interface IInteractable
    {
        float InteractionDistance { get => Settings.Settings.MAX_INTERACTION_DISTANCE; }

        string Name { get => ""; }

        void Interact();

        void ExitInteraction(Action onDone = null) {}

        void Select();

        void Deselect();
    }
}