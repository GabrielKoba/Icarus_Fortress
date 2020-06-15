using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class EnemySpawnBehaviour : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_enemies;
    [SerializeField] private GameConfig m_config;
    [SerializeField] private GameObject m_bossShipTransform;
    [SerializeField] private GameObject m_bossShipLogicGameObject;

    [SerializeField] private Animator m_bossCannonAnimator;
    [SerializeField] private Animator m_bossSmokeAnimator;
    [SerializeField] private GameObject m_enemyContainer;

    public bool ShouldSpawnEnemies = true;

    private float m_currentMinDelay;
    private float m_currentMaxDelay;

    private bool m_uppedBossDifficulty = false;

    private Coroutine m_enemyCoroutine;

    private readonly float[] m_weights = new float[3];
    private const float BOSS_SHIP_FINAL_POS_X = 7.4f;

    private float m_bossTimeAccumulator = 0f;

    [Header("Audio Settings")]
    [SerializeField]GameObject audioSource;    
    [FMODUnity.EventRef][SerializeField]string bossCannonSFX;
    [FMODUnity.EventRef][SerializeField]string bossCannonDistantSFX;

    // Start is called before the first frame update
    void Start()
    {
        m_weights[0] = m_config.WeightBasicEnemy;
        m_weights[1] = m_config.WeightMediumEnemy;
        m_weights[2] = m_config.WeightHardEnemy;

        m_currentMinDelay = m_config.MinDelay;
        m_currentMaxDelay = m_config.MaxDelay;

        m_enemyCoroutine = StartCoroutine(IntervalStartSpawnRandomEnemies());
        StartCoroutine(IncreaseDifficultyOverTime());

        m_bossTimeAccumulator = 0f;
    }

    private IEnumerator IncreaseDifficultyOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_config.TimeBetweenDifficultyIncrease);
            m_currentMinDelay = Mathf.Max(0, m_currentMinDelay - m_config.TimeDeltaDecreaseDelays);
            m_currentMaxDelay = Mathf.Max(9, m_currentMaxDelay - m_config.TimeDeltaDecreaseDelays);

            m_weights[1]++;
            m_weights[2]++;

            // Hack, don't show the debug log for all of the spawn points, they are the same
            if (gameObject.tag == "SpawnPointMiddle")
            {
                Debug.Log($"<color=green>[Increasing difficulty] Min: {m_currentMinDelay} Max: {m_currentMaxDelay}, Weights {m_weights[0]}, {m_weights[1]}, {m_weights[2]}</color>", this);
            }
        }
    }

    private IEnumerator AnimateAndActivateBossShip()
    {
        Debug.Log("Spawning boss!");
        m_bossShipTransform.GetComponentInChildren<BossShipBehaviour>().m_hasSpawned = true;
        while (m_bossShipTransform.transform.position.x > BOSS_SHIP_FINAL_POS_X)
        {
            m_bossShipTransform.transform.position -= new Vector3(5f * Time.deltaTime, 0f, 0f);
            yield return null;
        }
    }

    private void SetBossDifficulty()
    {
        StopCoroutine(m_enemyCoroutine);
        m_currentMinDelay = 2.5f;
        m_currentMaxDelay = 4.5f;
        m_weights[0] = 0;
        m_weights[1] = 0;
        m_weights[2] = 0;
        StartCoroutine(IntervalStartSpawnRandomEnemies());
    }

    private void Update()
    {
        if (m_bossTimeAccumulator > m_config.TimeBeforeBossSpawn && !m_uppedBossDifficulty)
        {
            m_uppedBossDifficulty = true;
            SetBossDifficulty();
        }

        if (m_bossTimeAccumulator > m_config.TimeBeforeBossSpawn && m_bossShipTransform != null && !m_bossShipTransform.GetComponentInChildren<BossShipBehaviour>().m_hasSpawned)
        {
            StartCoroutine(AnimateAndActivateBossShip());
        }

        m_bossTimeAccumulator += Time.deltaTime;
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
        while (ShouldSpawnEnemies )
        {
            var currentDelay = Random.Range(m_currentMinDelay, m_currentMaxDelay);
            yield return new WaitForSeconds(currentDelay);

            var index = GetRandomWeightedIndex();
            var go = Instantiate(m_enemies[index], transform);

            go.transform.SetParent(m_enemyContainer.transform);

            m_bossCannonAnimator.SetTrigger("Fire");
            m_bossSmokeAnimator.SetTrigger("Fire");

            if (m_bossShipLogicGameObject.GetComponentInChildren<BossShipBehaviour>().m_hasSpawned) {
                FMODUnity.RuntimeManager.PlayOneShot(bossCannonSFX, audioSource.transform.position);
            }
            else {
                FMODUnity.RuntimeManager.PlayOneShot(bossCannonDistantSFX, audioSource.transform.position);
            }
        }
    }
}
