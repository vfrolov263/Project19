using Assets._Project.Scripts.Gameplay.Graphics;
using Assets._Project.Scripts.Player;
using Assets._Project.Scripts.Systems.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Assets._Project.Scripts.Initialization
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private GameplayEntryPoint gameplayEntryPoint;
        [SerializeField]
        private VolumesController _volumesController;
        [SerializeField]
        private GameObject _playerPrefab;
        [SerializeField]    
        private Transform _playerStart;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlayerFactory>(Lifetime.Scoped);
            builder.Register<InputController>(Lifetime.Scoped);
            builder.Register(c => c.Resolve<PlayerFactory>().CreatePlayer(_playerPrefab, _playerStart),
                Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.RegisterComponent(_volumesController);
            builder.RegisterComponent(gameplayEntryPoint).AsImplementedInterfaces().AsSelf();
        }
    }
}
