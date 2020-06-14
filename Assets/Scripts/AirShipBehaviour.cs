using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirShipBehaviour : MonoBehaviour
{
    [SerializeField] private GameConfig m_config;

    private float m_lives;

    public void Start()
    {
        m_lives = m_config.PlayerStartingLives;
    }

    public void OnBulletHit()
    {
        m_lives--;
        GetComponent<CameraShake>().shakeDuration = 0.2f;

        if (m_lives == 0)
        {
            StartCoroutine(AnimateShipDeath());
        }
    }


    private IEnumerator AnimateShipDeath()
    {
        while (true)
        {
            this.transform.position += new Vector3(0f, 5f * Time.deltaTime, 0f);
        }
    }
}
