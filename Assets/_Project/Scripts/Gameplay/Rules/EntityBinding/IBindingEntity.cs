using System;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Rules.EntityBinding
{
    public interface IBindingEntity
    {
        public event Action EntityStateChanged;
        public void OnInfluencingEntityStateChanged(IBindingEntity entity) {}
    }
}
