using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyCounter : MonoBehaviour
{
    public TMP_Text enemyCounterText; 

    void Start()
    {
        UpdateEnemyCounterText();
    }

    void Update()
    {
        UpdateEnemyCounterText();
    }

    void UpdateEnemyCounterText()
    {
        int currentEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemyCounterText.text ="Enemies left:" + currentEnemies + "/" + EnemySpawner.totalEnemies;
    }
}