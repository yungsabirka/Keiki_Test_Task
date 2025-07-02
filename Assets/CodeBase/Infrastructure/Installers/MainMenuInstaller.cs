using CodeBase.Game.EntryPoints;
using CodeBase.UI.Screens.MainMenu;
using CodeBase.UI.Screens.MainMenu.Factory;
using UnityEngine;
using Zenject;
namespace CodeBase.Infrastructure.Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuEntryPoint _entryPoint;
        
        public override void InstallBindings()
        {
            InstallMenuElementsFactory();
            InstallMainMenu();
            InstallEntryPoint();
        }

        private void InstallMainMenu()
        {
            Container.Bind<MainMenuModel>().AsSingle();
            Container.Bind<MainMenuViewModel>().AsSingle();
        }
        
        private void InstallEntryPoint() => 
            Container
                .BindInstance(_entryPoint)
                .AsSingle();
        
        private void InstallMenuElementsFactory() =>
            Container
                .BindInterfacesTo<MenuElementsFactory>()
                .AsSingle();
    }
}