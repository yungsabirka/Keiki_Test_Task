using System;
using System.Collections.Generic;
using UnityEngine;
namespace CodeBase.Data.Levels
{
    [Serializable]
    public class LevelData
    {
        [field: SerializeField] public LevelType LevelType { get; private set; }
        [field: SerializeField] public Sprite LevelSprite { get; private set; }
        [field: SerializeField] public int FilledPartsCount { get; private set; }
        [field: SerializeField] public List<Color> LevelColors { get; private set; }
    }
}