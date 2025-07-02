using Cysharp.Threading.Tasks;
namespace CodeBase.Infrastructure.UI
{
    public interface IUIRootFactory
    {
        UniTask<UIRoot> GetUIRoot();

        UniTask<TScreen> CreateScreen<TScreen>() where TScreen : ScreenViewBase;

        UniTask<TPopup> CreatePopup<TPopup>() where TPopup : PopupViewBase;
    }
}