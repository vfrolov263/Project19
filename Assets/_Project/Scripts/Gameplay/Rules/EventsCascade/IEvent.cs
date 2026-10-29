using System;

namespace Assets._Project.Scripts.Gameplay.Rules.EventsCascade
{
    public interface IEvent
    {
        event Action<IEvent> Started;
        event Action<IEvent> Finished;
        event Action<IEvent> Interrupted;

        void Start() {}
    }
}