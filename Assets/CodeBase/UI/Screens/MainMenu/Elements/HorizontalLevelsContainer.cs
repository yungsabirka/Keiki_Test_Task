using CodeBase.Data.Levels;
using CodeBase.UI.Tools.Scrolls;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace CodeBase.UI.Screens.MainMenu.Elements
{
    public class HorizontalLevelsContainer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelsTitle;
        [SerializeField] private Transform _levelsContainerContent;
        [SerializeField] private ScrollBridge _scrollBridge;
        
        public LevelType LevelType { get; private set; }
        
        public void SetLevelType(LevelType levelType) =>
            LevelType = levelType;
        
        public void AddLevelPage(LevelPageView levelPage)
        {
            levelPage.transform.SetParent(_levelsContainerContent);
            levelPage.gameObject.SetActive(true);
        }

        public void SetLevelsTitle(string levelsTitle) => 
            _levelsTitle.text = levelsTitle;

        public void SetParentScroll(ScrollRect scrollView) =>
            _scrollBridge.SetParentScroll(scrollView);
    }
}