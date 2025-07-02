using UnityEngine;
namespace CodeBase.Services.ObjectsPool
{
    public interface IObjectsPool
    {
        void Initialize();

        T GetObject<T>(T prefab) where T : MonoBehaviour, IPoolable;

        void ClearPool();
    }
}