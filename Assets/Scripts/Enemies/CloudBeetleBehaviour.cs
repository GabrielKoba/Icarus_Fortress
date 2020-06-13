using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloudBeetleBehaviour : MonoBehaviour
{
    [SerializeField] private GameConfig m_config;
    [SerializeField] private GameEventStringParam m_enemyHitPlayer;

    private Transform[] m_spawnPointWorldPositions;

    private bool m_isJumpingLanes = false;

    public void Start()
    {
        StartCoroutine(SwitchLanes());

        m_spawnPointWorldPositions = new Transform[3];
        m_spawnPointWorldPositions[0] = GameObject.FindGameObjectWithTag("SpawnPointBottom").transform;
        m_spawnPointWorldPositions[1] = GameObject.FindGameObjectWithTag("SpawnPointMiddle").transform;
        m_spawnPointWorldPositions[2] = GameObject.FindGameObjectWithTag("SpawnPointTop").transform;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag != "Enemy")
        {
            Destroy(gameObject);
        }

        if (other.collider.tag == "AirShip")
        {
            m_enemyHitPlayer.Raise(string.Empty);
        }
    }

    private IEnumerator SwitchLanes()
    {
        while (true)
        {
            var randomDelay = Random.Range(m_config.CloudBeetleSwitchLanesFrequencyMin, m_config.CloudBeetleSwitchLanesFrequencyMax);
            yield return new WaitForSeconds(randomDelay);

            if (m_isJumpingLanes)
            {
                continue;
            }

            // In middle lane
            if (Math.Abs(transform.position.y - m_spawnPointWorldPositions[1].position.y) < 0.1f)
            {
                var random = Random.Range(0, 1);

                if (random == 0)
                {
                    StartCoroutine(AnimateVerticallyDown(m_spawnPointWorldPositions[0].position.y));
                }
                else
                {
                    StartCoroutine(AnimateVerticallyUp(m_spawnPointWorldPositions[2].position.y));
                }
            }

            // In bottom lane
            if (Math.Abs(transform.position.y - m_spawnPointWorldPositions[0].position.y) < 0.1f)
            {
                StartCoroutine(AnimateVerticallyUp(m_spawnPointWorldPositions[1].position.y));
            }

            // In top lane
            if (Math.Abs(transform.position.y - m_spawnPointWorldPositions[2].position.y) < 0.1f)
            {
                StartCoroutine(AnimateVerticallyDown(m_spawnPointWorldPositions[1].position.y));
            }

            yield return null;
        }
    }

    private IEnumerator AnimateVerticallyUp(float endPosition)
    {
        m_isJumpingLanes = true;

        while (transform.position.y < endPosition)
        {
            transform.position += new Vector3(0f, m_config.CloudBeetleSwitchLanesSpeed * Time.deltaTime, 0f);
            yield return null;
        }

        m_isJumpingLanes = false;
    }

    private IEnumerator AnimateVerticallyDown(float endPosition)
    {
        m_isJumpingLanes = true;

        while (transform.position.y > endPosition)
        {
            transform.position -= new Vector3(0f, m_config.CloudBeetleSwitchLanesSpeed * Time.deltaTime, 0f);
            yield return null;
        }

        m_isJumpingLanes = false;
    }

    private void Update()
    {
        this.transform.position -= new Vector3(m_config.CloudBeetleMoveSpeed * Time.deltaTime, 0f, 0f);
    }
}
