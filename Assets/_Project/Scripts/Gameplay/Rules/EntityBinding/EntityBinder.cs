using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Rules.EntityBinding
{
    public class EntityBinder : IDisposable
    {
        private Dictionary<int, IEntityState> _entities = new();
        private Dictionary<int, IEntityState> _waitingList = new();

        public void AddBinding(IEntityState entity)
        {
            Debug.Log($"try add binding id {entity.Id}");
            if (entity == null)
                throw new ArgumentException("Entity is null.");

            if (_entities.ContainsKey(entity.Id))
                throw new ArgumentException($"Entity with id {entity.Id} already registered.");

            _entities.Add(entity.Id, entity);

            if (entity.InfluencingId != null)
            {
                if (_entities.TryGetValue(entity.InfluencingId.Value, out var bindingState))
                {
                    bindingState.StateChanged += entity.OnInfluencingEntityStateChanged;
                }
                else
                {
                    if (!_waitingList.TryAdd(entity.InfluencingId.Value, entity))
                        throw new Exception("Try add duplicate binding.");
                }
            }

            CheckAwaiters(entity);
        }

        public void RemoveBinding(IEntityState entity)
        {
            if (entity == null)
                throw new ArgumentException("Entity is null.");

            if (!_entities.ContainsKey(entity.Id))
                return;

            if (entity.InfluencingId != null)
            {
                if (_entities.TryGetValue(entity.InfluencingId.Value, out var bindingState))
                {
                    bindingState.StateChanged -= entity.OnInfluencingEntityStateChanged;
                }
                else
                {
                    _waitingList.Remove(entity.InfluencingId.Value);
                }
            }

            _entities.Remove(entity.Id);
        }

        public void Dispose()
        {
            var entitiesToRemove =  _entities.Values.ToList();
            
            foreach (var entity in entitiesToRemove)
                RemoveBinding(entity);
        }

        private void CheckAwaiters(IEntityState newEntity)
        {
            if (_waitingList.TryGetValue(newEntity.Id, out var entity))
            {
                newEntity.StateChanged += entity.OnInfluencingEntityStateChanged;
                _waitingList.Remove(newEntity.Id);
            }
        }
    }
}
