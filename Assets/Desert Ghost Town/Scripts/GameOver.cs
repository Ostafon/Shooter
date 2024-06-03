using UnityEngine;
using UnityEngine.SceneManagement;

namespace Desert_Ghost_Town.Scripts
{
    public class GameOver : MonoBehaviour
    {
        public string gameOverSceneName = "GameOver";

        public void ShowGameOverScene()
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
        
    }
}