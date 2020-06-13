using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnBehaviour : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_enemies;
    [SerializeField] private GameConfig m_config;

    public bool ShouldSpawnEnemies = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IntervalStartSpawnRandomEnemies());
    }

    private IEnumerator IntervalStartSpawnRandomEnemies()
    {
        var delay = 2f;

        while (ShouldSpawnEnemies)
        {
            var currentDelay = Random.Range(m_config.MinDelay, m_config.MaxDelay);
            yield return new WaitForSeconds(currentDelay);

            var random = Random.Range(0, m_enemies.Count);
            Instantiate(m_enemies[random], transform);
        }
    }
}
