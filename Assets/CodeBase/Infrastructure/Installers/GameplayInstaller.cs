using CodeBase.Game.EntryPoints;
using CodeBase.Services.Timer;
using CodeBase.UI.Popups.FingerHint;
using CodeBase.UI.Screens.Gameplay;
using CodeBase.UI.Screens.Gameplay.Factory;
using CodeBase.UI.Screens.Gameplay.Services.AudioService;
using CodeBase.UI.Screens.Gameplay.Services.FillPathSolver;
using CodeBase.UI.Screens.Gameplay.Services.HintsPath;
using CodeBase.UI.Screens.Gameplay.Services.HintsTimerService;
using UnityEngine;
using Zenject;
namespace CodeBase.Infrastructure.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private GameplayEntryPoint _entryPoint;

        public override void InstallBindings()
        {
            InstallGameplayAudioService();
            InstallGameplayElementsFactory();
            InstallFillAmountCalculator();
            InstallHintsTimerService();
            InstallTimer();
            InstallHintsPathService();
            InstallGameplay();
            InstallFingerPopup();
            InstallEntryPoint();
        }

        private void InstallGameplay()
        {
            Container
                .Bind<GameplayModel>()
                .AsSingle();
            Container
                .Bind<GameplayViewModel>()
                .AsSingle();
        }

        private void InstallFingerPopup()
        {
            Container
                .Bind<FingerHintModel>()
                .AsSingle();
            Container
                .Bind<FingerHintViewModel>()
                .AsSingle();
        }

        private void InstallEntryPoint() =>
            Container
                .BindInstance(_entryPoint)
                .AsSingle();

        private void InstallGameplayElementsFactory() =>
            Container
                .BindInterfacesTo<GameplayElementsFactory>()
                .AsSingle();

        private void InstallFillAmountCalculator() =>
            Container
                .BindInterfacesTo<FillPathSolver>()
                .AsTransient();

        private void InstallHintsPathService() =>
            Container
                .BindInterfacesTo<HintsPathService>()
                .AsSingle();

        private void InstallTimer() =>
            Container
                .BindInterfacesTo<Timer>()
                .AsSingle();

        private void InstallHintsTimerService() =>
            Container
                .BindInterfacesTo<HintsTimerService>()
                .AsSingle();

        private void InstallGameplayAudioService() =>
            Container
                .BindInterfacesTo<GameplayAudioService>()
                .AsSingle();
    }
}