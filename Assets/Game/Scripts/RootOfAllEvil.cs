using System.Collections.Generic;
using Game.Scripts.Levels;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts
{
    public class RootOfAllEvil : MonoBehaviour
    {
        [SerializeField] private List<string> _levels;
        private int currentLevelIndex;
        
        public void Awake()
        {
            currentLevelIndex = 0;
            DontDestroyOnLoad(gameObject);
        }

        private void LoadNextLevel()
        {
            if (currentLevelIndex == 0)
            {
                SceneManager.LoadScene(_levels[0]);
                
            }
            else
            {

                SceneManager.UnloadSceneAsync(currentLevelIndex);
                SceneManager.LoadScene(++currentLevelIndex);
            }

            ExitTrigger.ExitTriggered += OnExitTriggered;
        }
        
        private void OnExitTriggered()
        {
            ExitTrigger.ExitTriggered -= OnExitTriggered;
            LoadNextLevel();
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
