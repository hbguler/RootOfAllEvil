using System.Collections;
using System.Collections.Generic;
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
            PlayerBehaviour.CharacterDied += OnCharacterDied;
        }

        private void LoadNextLevel()
        {
            if (currentLevelIndex == 0)
            {
                SceneManager.LoadScene(1);
                currentLevelIndex = 1;
            }
            else
            {
                SceneManager.UnloadSceneAsync(currentLevelIndex);
                SceneManager.LoadScene(++currentLevelIndex);
            }
            
            ExitTrigger.ExitTriggered += OnExitTriggered;
        }

        private void OnCharacterDied()
        {
            SceneManager.UnloadSceneAsync(currentLevelIndex);
            SceneManager.LoadScene(currentLevelIndex);        
        }

        private void OnExitTriggered()
        {
            ExitTrigger.ExitTriggered -= OnExitTriggered;

            StartCoroutine(LoadLevelCoroutine());
        }

        private IEnumerator LoadLevelCoroutine()
        {
            yield return new WaitForSeconds(2f);
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
