using System;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Animation
{
    public class AnimationEventsHandler : MonoBehaviour
    {
        public Action<string> OnEvent;

        public void EventHandler(string name)
        {
            OnEvent?.Invoke(name);
        }
    }
}
