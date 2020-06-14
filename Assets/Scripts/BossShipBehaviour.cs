using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShipBehaviour : MonoBehaviour
{
    public bool m_hasSpawned = false;
    private int m_lives = 4;
    [SerializeField] List<GameObject> airShipRopes;
    [SerializeField] private GameConfig m_config;

    [Header("Audio Settings")]
    [FMODUnity.EventRef][SerializeField]string ropeSnapSFX;
    [FMODUnity.EventRef][SerializeField]string AirshipHitSFX;


    private int m_numIntermedieteHits = 0;

    public void OnBossHit(string thingThatHit)
    {
        if (m_lives == 0 || !m_hasSpawned)
        {
            return;
        }

        GetComponent<CameraShake>().shakeDuration = 0.2f;
        FMODUnity.RuntimeManager.PlayOneShot(AirshipHitSFX, transform.position);

        if (thingThatHit == "BigCannonBall" || m_numIntermedieteHits == m_config.NumNormalHitsBeforeBossTakesLife)
        {
            m_numIntermedieteHits = 0;

            m_lives--;

            //Sound
            FMODUnity.RuntimeManager.PlayOneShot(ropeSnapSFX, transform.position);


            airShipRopes[m_lives].GetComponent<Animator>().SetTrigger("Snapped");
        }
        else
        {
            m_numIntermedieteHits++;
        }

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
        StartCoroutine(GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, "EndScene"));
    }
}
