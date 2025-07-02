using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Data.Levels;
using CodeBase.Infrastructure.States;
using CodeBase.Infrastructure.States.Core;
using CodeBase.Services.LevelsProvider;
using Cysharp.Threading.Tasks;
using UnityEngine;
namespace CodeBase.UI.Screens.MainMenu
{
    public class MainMenuModel
    {
        private readonly IStateMachine _gameStateMachine;
        private readonly ILevelsDataProvider _levelsDataProvider;

        public IReadOnlyList<LevelData> LevelsData => _levelsDataProvider.LevelsData;
        
        public MainMenuModel(IStateMachine gameStateMachine, ILevelsDataProvider levelsDataProvider)
        {
            _gameStateMachine = gameStateMachine;
            _levelsDataProvider = levelsDataProvider;
        }

        public async UniTask LoadGameplay(LevelType levelType, Color color)
        {
            var enterParams = new LevelEnterParams(color, levelType);
            await _gameStateMachine.Enter<LoadGameplayState, LevelEnterParams>(enterParams);
        }
    }
}