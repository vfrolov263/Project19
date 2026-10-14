using System;
using System.Collections.Generic;
using Assets._Project.Scripts.Gameplay.Rules.EntityBinding;
using Assets._Project.Scripts.Gameplay.Rules.Physics;
using Assets._Project.Scripts.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameplayEntryPoint : MonoBehaviour, IStartable
{
    [SerializeField]
    private List<BindingEntity> _bindingEntities;

    private EntityBinder _entityBinder = new();

    void IStartable.Start()
    {
        foreach (BindingEntity entity in _bindingEntities)
            _entityBinder.AddBinding(entity);
    }

    private void OnDestroy()
    {
        _entityBinder.Dispose();
    }
}
