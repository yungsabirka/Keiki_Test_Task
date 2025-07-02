using CodeBase.Infrastructure.Scenes;
using CodeBase.Infrastructure.States.Core;
using CodeBase.Infrastructure.UI;
using CodeBase.Services.AssetsSystem;
using CodeBase.Services.AudioSystem.AudioSystem;
using CodeBase.Services.LevelsProvider;
using CodeBase.Services.ObjectsPool;
using Zenject;
namespace CodeBase.Infrastructure.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallGameStateMachine();
            InstallStatesFactory();
            InstallAssetsProvider();
            InstallUIFactory();
            InstallSceneLoader();
            InstallLevelsDataProvider();
            InstallObjectsPool();
            InstallAudioSystem();
        }

        private void InstallStatesFactory() =>
            Container
                .BindInterfacesTo<GameStatesInitializer>()
                .AsSingle();

        private void InstallGameStateMachine() =>
            Container
                .BindInterfacesTo<GameStateMachine>()
                .AsSingle();

        private void InstallUIFactory() =>
            Container
                .BindInterfacesTo<UIRootFactory>()
                .AsSingle();

        private void InstallAssetsProvider() =>
            Container
                .BindInterfacesTo<AssetsProvider>()
                .AsSingle();

        private void InstallSceneLoader() =>
            Container
                .BindInterfacesTo<SceneLoader>()
                .AsSingle();

        private void InstallLevelsDataProvider() =>
            Container
                .BindInterfacesTo<LevelsDataProvider>()
                .AsSingle();

        private void InstallObjectsPool() =>
            Container
                .BindInterfacesTo<ObjectsPool>()
                .AsSingle();

        private void InstallAudioSystem() =>
            Container
                .BindInterfacesTo<AudioSystem>()
                .AsSingle();
    }
}