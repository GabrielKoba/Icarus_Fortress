using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnBehaviour : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_enemies;
    [SerializeField] private GameConfig m_config;

    public bool ShouldSpawnEnemies = true;

    private float m_currentMinDelay;
    private float m_currentMaxDelay;

    // Start is called before the first frame update
    void Start()
    {
        m_currentMinDelay = m_config.MinDelay;
        m_currentMaxDelay = m_config.MaxDelay;

        StartCoroutine(IntervalStartSpawnRandomEnemies());
        StartCoroutine(IncreaseDifficultyOverTime());
    }

    private IEnumerator IncreaseDifficultyOverTime()
     {
         yield return new WaitForSeconds(m_config.TimeBetweenDifficultyIncrease);

         m_currentMinDelay--;
         m_currentMaxDelay--;

         Debug.Log($"<color=green>[Increasing difficulty] Min: {m_currentMinDelay} Max: {m_currentMaxDelay}</color>", this);
     }

    private IEnumerator IntervalStartSpawnRandomEnemies()
    {
        while (ShouldSpawnEnemies)
        {
            var currentDelay = Random.Range(m_config.MinDelay, m_config.MaxDelay);
            yield return new WaitForSeconds(currentDelay);

            var random = Random.Range(0, m_enemies.Count);
            Instantiate(m_enemies[random], transform);
        }
    }
}
