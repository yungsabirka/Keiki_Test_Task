using UnityEngine;
using Zenject;
namespace CodeBase.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private GameBootstrapper _gameBootstrapper;
        
        public override void InstallBindings() =>
            Container.BindInstance(_gameBootstrapper).AsSingle();
    }
}