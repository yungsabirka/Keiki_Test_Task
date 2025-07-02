using System.Collections.Generic;
using CodeBase.Data.Levels;
using CodeBase.Services.AssetsSystem;
using Cysharp.Threading.Tasks;
using static CodeBase.Services.AssetsSystem.Data.AssetsSystemConstants;
namespace CodeBase.Services.LevelsProvider
{
    public class LevelsDataProvider : ILevelsDataProvider
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly List<LevelData> _levelsData = new();

        public IReadOnlyList<LevelData> LevelsData => _levelsData;

        public LevelsDataProvider(IAssetsProvider assetsProvider) =>
            _assetsProvider = assetsProvider;

        public async UniTask Initialize() =>
            await LoadLevelsData();

        private async UniTask LoadLevelsData()
        {
            var levelsData = await _assetsProvider.LoadAsync<LevelsData>(LevelsDataKey);
            foreach (var level in levelsData.Levels)
                _levelsData.Add(level);
        }
    }
}