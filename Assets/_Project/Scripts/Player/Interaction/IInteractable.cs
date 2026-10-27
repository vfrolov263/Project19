using System;

namespace Assets._Project.Scripts.Player.Interaction
{
    public interface IInteractable
    {
        float InteractionDistance { get => Settings.Settings.DEFAULT_INTERACTION_DISTANCE; }

        string Name { get => ""; }

        bool Interact();

        void ExitInteraction(Action onDone = null) {}

        void Select() {}

        void Deselect() {}
    }
}