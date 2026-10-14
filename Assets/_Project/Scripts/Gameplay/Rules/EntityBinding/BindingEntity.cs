using System;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Rules.EntityBinding
{
    public class BindingEntity: MonoBehaviour, IEntityState
    {
        [field: SerializeField]
        public int Id { get; private set; }

        [SerializeField]
        private int _influencingId;

        [SerializeField]
        private bool _hasInfluencing;

        public int? InfluencingId { get; private set; }

        public event Action<IEntityState> StateChanged;

        [SerializeField]
        private MonoBehaviour _entity;

        public IBindingEntity Entity { get; private set; }

        public void OnInfluencingEntityStateChanged(IEntityState influencingEntity)
        {
            Entity.OnInfluencingEntityStateChanged(influencingEntity.Entity);
        }

        private void Awake()
        {
            if (_entity is not IBindingEntity bindingEntity)
                throw new ArgumentException("Binding entity serialized with non BindingEntity");
            
            Entity = bindingEntity;
            InfluencingId = _hasInfluencing ? _influencingId : null;
        }

        private void OnEnable()
        {
            Entity.EntityStateChanged += OnStateChanged;
        }

        private void OnDisable()
        {
            Entity.EntityStateChanged -= OnStateChanged;
        }

        private void OnStateChanged()
        {
            StateChanged?.Invoke(this);
        }
    }
}