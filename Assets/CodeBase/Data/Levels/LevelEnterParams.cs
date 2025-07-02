using UnityEngine;
namespace CodeBase.Data.Levels
{
    public class LevelEnterParams
    {
        public Color LevelColor { get; private set; }
        public LevelType LevelType { get; private set; }
        
        public LevelEnterParams(Color levelColor, LevelType levelType)
        {
            LevelColor = levelColor;
            LevelType = levelType;
        }
    }
}