using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
namespace CodeBase.Services.AssetsSystem
{
    public class AssetsProvider : IAssetsProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new();

        public async UniTask<T> LoadAsync<T>(string assetPath) where T : class
        {
            if (_completedCache.TryGetValue(assetPath, out AsyncOperationHandle completedHandle))
                return await UniTask.FromResult(completedHandle.Result as T);

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetPath);
            handle.Completed += OnHandleCompleted;
            AddHandle(assetPath, handle);

            return await handle.ToUniTask();

            void OnHandleCompleted(AsyncOperationHandle<T> operationHandle)
            {
                _completedCache.Add(assetPath, operationHandle);
                handle.Completed -= OnHandleCompleted;
            }
        }

        public void CleanUp()
        {
            foreach (var handles in _handles.Values)
            foreach (var handle in handles)
                if (!_completedCache.ContainsValue(handle))
                    Addressables.Release(handle);

            _handles.Clear();
            _completedCache.Clear();
        }

        private void AddHandle<T>(string assetPath, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_handles.TryGetValue(assetPath, out List<AsyncOperationHandle> resourceHandlers))
            {
                resourceHandlers = new List<AsyncOperationHandle>();
                _handles.Add(assetPath, resourceHandlers);
            }
            resourceHandlers.Add(handle);
        }
    }
}