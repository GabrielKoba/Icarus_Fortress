using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class EnemySpawnBehaviour : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_enemies;
    [SerializeField] private GameConfig m_config;

    public bool ShouldSpawnEnemies = true;

    private float m_currentMinDelay;
    private float m_currentMaxDelay;

    private readonly float[] m_weights = new float[3];

    // Start is called before the first frame update
    void Start()
    {
        m_weights[0] = m_config.WeightBasicEnemy;
        m_weights[1] = m_config.WeightMediumEnemy;
        m_weights[2] = m_config.WeightHardEnemy;

        m_currentMinDelay = m_config.MinDelay;
        m_currentMaxDelay = m_config.MaxDelay;

        StartCoroutine(IntervalStartSpawnRandomEnemies());
        StartCoroutine(IncreaseDifficultyOverTime());
    }

    private IEnumerator IncreaseDifficultyOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_config.TimeBetweenDifficultyIncrease);
            m_currentMinDelay -= m_config.TimeDeltaDecreaseDelays;
            m_currentMaxDelay -= m_config.TimeDeltaDecreaseDelays;

            m_weights[1]++;
            m_weights[2]++;

            // Hack, don't show the debug log for all of the spawn points, they are the same
            if (gameObject.tag == "SpawnPointMiddle")
            {
                Debug.Log($"<color=green>[Increasing difficulty] Min: {m_currentMinDelay} Max: {m_currentMaxDelay}, Weights {m_weights[0]}, {m_weights[1]}, {m_weights[2]}</color>", this);
            }
        }
    }

    private int GetRandomWeightedIndex()
    {
        var weightSum = m_weights.Sum();
        var randomIndex = Random.Range(0, weightSum);

        if (randomIndex <= m_weights[0])
        {
            return 0;
        }

        if (randomIndex <= m_weights[0] + m_weights[1])
        {
            return 1;
        }

        return 2;
    }

    private IEnumerator IntervalStartSpawnRandomEnemies()
    {
        while (ShouldSpawnEnemies)
        {
            var currentDelay = Random.Range(m_config.MinDelay, m_config.MaxDelay);
            yield return new WaitForSeconds(currentDelay);

            var index = GetRandomWeightedIndex();
            Instantiate(m_enemies[index], transform);
        }
    }
}
