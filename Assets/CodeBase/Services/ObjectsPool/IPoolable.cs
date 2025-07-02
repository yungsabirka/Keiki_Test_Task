using System;
using UnityEngine;
namespace CodeBase.Services.ObjectsPool
{
    public interface IPoolable
    {
        GameObject GameObject { get; }
        
        event Action<IPoolable> ReturnRequested;

        void ResetPoolable();
    }
}