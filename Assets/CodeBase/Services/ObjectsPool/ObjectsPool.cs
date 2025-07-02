using System.Collections.Generic;
using UnityEngine;
namespace CodeBase.Services.ObjectsPool
{
    public class ObjectsPool : IObjectsPool
    {
        private Dictionary<GameObject, PoolTask> _activePoolTasks;
        private Transform _objectPoolTransform;

        public void Initialize()
        {
            _activePoolTasks = new Dictionary<GameObject, PoolTask>();
            _objectPoolTransform = new GameObject().transform;
        }
        
        public T GetObject<T>(T prefab) where T : MonoBehaviour, IPoolable 
        {
            if (!_activePoolTasks.TryGetValue(prefab.GameObject, out var poolTask))
            {
                AddTaskToPool(prefab, out poolTask);
            }

            return poolTask.GetFreeObject(prefab);
        }

        public void ClearPool()
        {
            foreach (var poolTask in _activePoolTasks.Values)
                poolTask.ClearPool();
            _activePoolTasks.Clear();
        }
        
        private void AddTaskToPool<T>(T prefab, out PoolTask poolTask) where T : MonoBehaviour, IPoolable
        {
            GameObject container = new GameObject
            {
                name = $"{prefab.name}s_pool"
            };
            container.transform.SetParent((_objectPoolTransform));
            poolTask = new PoolTask(container.transform);
            _activePoolTasks.Add(prefab.GameObject, poolTask);
        }
    }
}