using System;
using Assets._Project.Scripts.Gameplay.Props;
using Assets._Project.Scripts.Gameplay.Rules.EventsCascade;
using DG.Tweening;
using UnityEngine;

namespace  Assets._Project.Scripts.Gameplay.Common
{
    public class DoorUnlocker : MonoBehaviour, IEvent
    {
        [SerializeField]
        private DoorController _door;
        
        public event Action<IEvent> Started, Finished, Interrupted;

        void IEvent.Start()
        {
            Started?.Invoke(this);
            _door.IsLocked = false;
            Finished?.Invoke(this);
        }
    }
}