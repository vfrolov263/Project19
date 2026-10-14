using System;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Rules.EntityBinding
{
    public interface IEntityState
    {
        public int Id { get; }
        public int? InfluencingId { get; }
        public IBindingEntity Entity { get; }
        event Action<IEntityState> StateChanged;
        public void OnInfluencingEntityStateChanged(IEntityState influencingEntity);
    }
}