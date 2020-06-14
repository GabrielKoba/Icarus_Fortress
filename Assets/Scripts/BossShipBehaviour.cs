using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShipBehaviour : MonoBehaviour
{
    public bool m_hasSpawned = false;
    private int m_lives = 4;
    [SerializeField] List<GameObject> airShipRopes;

    public void OnBossHit(string thingThatHit)
    {
        if (m_lives == 0 || !m_hasSpawned)
        {
            return;
        }

        if (thingThatHit == "BigCannonBall")
        {
            airShipRopes[m_lives - 1].GetComponent<Animator>().SetTrigger("Snapped");

            var lives = m_lives -= 2;

            if (lives >= 0)
            {
                airShipRopes[lives].GetComponent<Animator>().SetTrigger("Snapped");
            }

            m_lives = Math.Max(lives, 0);
        }
        else
        {
            m_lives--;
            airShipRopes[m_lives].GetComponent<Animator>().SetTrigger("Snapped");
        }

        GetComponent<CameraShake>().shakeDuration = 0.2f;

        if (m_lives == 0)
        {
            StartCoroutine(AnimateShipDeath());
            StartCoroutine(LoadSceneAgainAfterDelay());
        }
    }

    private IEnumerator AnimateShipDeath()
    {
        GetComponent<CameraShake>().enabled = false;
        while (true)
        {
            this.transform.position -= new Vector3(0f, 5f * Time.deltaTime, 0f);
            yield return null;
        }
    }

    private IEnumerator LoadSceneAgainAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, "SampleScene"));
    }
}
