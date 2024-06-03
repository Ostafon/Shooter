using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseCounter : MonoBehaviour
{
    public static WinLoseCounter instance;

    private int wins = 0;
    private int losses = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
        if (scene.name == "WinMenu")
        {
            IncrementWins();
        }
        else if (scene.name == "GameOver")
        {
            IncrementLosses();
        }
    }

    public int GetWins()
    {
        return wins;
    }

    public int GetLosses()
    {
        return losses;
    }

    public void IncrementWins()
    {
        wins++;
    }

    public void IncrementLosses()
    {
        losses++;
    }
}