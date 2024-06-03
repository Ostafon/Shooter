using UnityEngine;
using UnityEngine.SceneManagement;

namespace Desert_Ghost_Town.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene(0);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
