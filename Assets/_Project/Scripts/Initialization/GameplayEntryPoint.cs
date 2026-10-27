using System;
using System.Collections.Generic;
using System.Linq;
using Assets._Project.Scripts.Gameplay.Rules;
using Assets._Project.Scripts.Gameplay.Rules.EntityBinding;
using Assets._Project.Scripts.Gameplay.Rules.Physics;
using Assets._Project.Scripts.Gameplay.Rules.Progress;
using Assets._Project.Scripts.Gameplay.Rules.TriggerHandler;
using Assets._Project.Scripts.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameplayEntryPoint : MonoBehaviour, IStartable
{
    [SerializeField]
    private ProgressConfig _progressConfig;

    [SerializeField]
    private List<BindingEntity> _bindingEntities;

    private EntityBinder _entityBinder = new();
    private ProgressController _progressController;

    [Inject]
    public void Construct(ProgressController progressController)
    {
        _progressController = progressController;
    }

    void IStartable.Start()
    {
        foreach (BindingEntity entity in _bindingEntities)
            _entityBinder.AddBinding(entity);

        InitProgressRules();
    }

    private void InitProgressRules()
    {
        if (_progressConfig == null)
            return;

        var handlers = FindObjectsByType<SimpleTriggerHandler>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();

        foreach (var rule in _progressConfig)
        {
            ITriggerHandler handler = handlers.First(x => x.Name == rule.handlerName);

            if (handler != null)
                _progressController.AddProgressRule(handler, rule.triggers);
        }
    }

    private void OnDestroy()
    {
        _entityBinder.Dispose();
    }
}
