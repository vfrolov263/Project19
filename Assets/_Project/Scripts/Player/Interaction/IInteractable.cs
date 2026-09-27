namespace Assets._Project.Scripts.Player.Interaction
{
    public interface IInteractable
    {
        float InteractionDistance { get; }

        void Interact();

        void Select();

        void Deselect();
    }
}