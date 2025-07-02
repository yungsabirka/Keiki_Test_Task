using Cysharp.Threading.Tasks;
namespace CodeBase.Services.AssetsSystem
{
    public interface IAssetsProvider
    {
        UniTask Initialize();
        
        UniTask<T> LoadAsync<T>(string assetPath) where T : class;

        void CleanUp();
    }
}