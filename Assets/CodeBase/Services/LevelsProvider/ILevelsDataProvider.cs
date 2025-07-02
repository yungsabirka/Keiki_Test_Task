using System.Collections.Generic;
using CodeBase.Data.Levels;
using Cysharp.Threading.Tasks;
namespace CodeBase.Services.LevelsProvider
{
    public interface ILevelsDataProvider
    {
        IReadOnlyList<LevelData> LevelsData { get; }

        UniTask Initialize();
    }
}