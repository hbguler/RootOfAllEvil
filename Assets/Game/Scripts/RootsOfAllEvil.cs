using System.Collections.Generic;
using Game.Scripts.Levels;
using UnityEngine;

namespace Game.Scripts
{
    public class RootsOfAllEvil : MonoBehaviour
    {
        [SerializeField] private List<LevelBehaviour> _levels;
        private LevelBehaviour _currentLevel;
        
        public void Awake()
        {
            _currentLevel = null;
        }

        private void LoadNextLevel()
        {
            if (_currentLevel == null)
            {
                _currentLevel = Instantiate(_levels[0]);
            }
            else
            {
                int indexOfCurrentLevel = _levels.FindIndex((lb) => lb == _currentLevel);
                indexOfCurrentLevel++;

                if (indexOfCurrentLevel < _levels.Count)
                {
                    Destroy(_currentLevel.gameObject);
                    _currentLevel = Instantiate(_levels[indexOfCurrentLevel]);
                }
            }

            _currentLevel.Initialize();
        }

        public void OnNewGameButtonClicked()
        {
            LoadNextLevel();
        }

        public void OnOptionsButtonClicked()
        {
            
        }

        public void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}
