using System.Collections.Generic;
using UnityEngine;
namespace CodeBase.Data.Levels
{
    [CreateAssetMenu(menuName = "GameData/LevelsData")]
    public class LevelsData : ScriptableObject
    {
        [field: SerializeField] public List<LevelData> Levels { get; private set; }
    }
}