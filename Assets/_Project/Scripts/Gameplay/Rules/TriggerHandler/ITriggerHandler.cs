namespace Assets._Project.Scripts.Gameplay.Rules.TriggerHandler
{
    public interface ITriggerHandler
    {
        string Name { get; }
        void Execute();
    }
}