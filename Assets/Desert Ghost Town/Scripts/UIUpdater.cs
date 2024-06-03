using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Desert_Ghost_Town.Scripts
{
    public class UIUpdater : MonoBehaviour
    {
        public TextMeshProUGUI winText;
        public TextMeshProUGUI loseText;

        private void Start()
        {
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            if (winText != null)
            {
                winText.text = "Wins: " + WinLoseCounter.instance.GetWins();
               
            }

            if (loseText != null)
            {
                loseText.text = "Losses: " + WinLoseCounter.instance.GetLosses();
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "MainScene")
            {
                UpdateCounter();
            }
        }
    }
}