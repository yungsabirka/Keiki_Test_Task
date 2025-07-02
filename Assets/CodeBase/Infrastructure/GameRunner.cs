using UnityEngine;
namespace CodeBase.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private GameBootstrapper _bootPrefab;

        private void Awake()
        {
            var boot = FindObjectOfType<GameBootstrapper>();
            
            if(boot == null)
                Instantiate(_bootPrefab);
        }
    }
}