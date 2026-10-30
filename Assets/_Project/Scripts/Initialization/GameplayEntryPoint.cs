using System;
using System.Collections.Generic;
using System.Linq;
using Assets._Project.Scripts.Gameplay.Rules;
using Assets._Project.Scripts.Gameplay.Rules.EntityBinding;
using Assets._Project.Scripts.Gameplay.Rules.Physics;
using Assets._Project.Scripts.Gameplay.Rules.Progress;
using Assets._Project.Scripts.Gameplay.Rules.TriggerHandler;
using Assets._Project.Scripts.Player;
using Assets._Project.Scripts.Player.Helpers;
using Assets._Project.Scripts.Tutorial;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameplayEntryPoint : MonoBehaviour, IStartable
{
    [SerializeField]
    private ProgressConfig _progressConfig;
    [SerializeField]
    private TriggersConfig _triggersConfig;
    [SerializeField]
    private CommentsConfig _commentsConfig;

    [SerializeField]
    private List<BindingEntity> _bindingEntities;
    [SerializeField]
    private TutorialView _tutorialView;

    private EntityBinder _entityBinder = new();
    private ProgressController _progressController;
    private PlayerController _playerController;

    [Inject]
    public void Construct(PlayerController playerController, ProgressController progressController)
    {
        _playerController = playerController;
        _progressController = progressController;
    }

    void IStartable.Start()
    {
        foreach (BindingEntity entity in _bindingEntities)
            _entityBinder.AddBinding(entity);

        InitProgressRules();
        CreateCommentator();
        InitTutorial();
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

        _progressController.AddTriggersDictionary(_triggersConfig.GetTriggersDictionary());
        _progressController.AddProgressRule(new GetGlassesTrigger(_playerController), "Лорнет?");
    }

    private void CreateCommentator()
    {
        PlayerCommentator commentator = new(_playerController, _progressController, _commentsConfig.GetCommentsDictionary());
    }

    private void InitTutorial()
    {
        TutorialController tutorial = new(_playerController, _progressController, _tutorialView);
    }

    private void OnDestroy()
    {
        _entityBinder.Dispose();
    }
}
