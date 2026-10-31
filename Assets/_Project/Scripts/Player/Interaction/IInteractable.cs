using System;

namespace Assets._Project.Scripts.Player.Interaction
{
    public interface IInteractable
    {
        float InteractionDistance { get => Settings.Settings.DEFAULT_INTERACTION_DISTANCE; }

        string Name { get => ""; }

        event Action ExitRequested;

        bool HasToInteract => true;

        bool HasToExamine => true;

        bool HasToDeepExamine => false;

        bool HasManualExit => true;
        
        bool IsNeedPointer  => false;

        bool Interact();

        void ExitInteraction(Action onDone = null) {}

        void Select() {}

        void Deselect() {}
    }
}