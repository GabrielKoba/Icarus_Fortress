using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallBehaviour : MonoBehaviour
{
    [SerializeField] private GameConfig m_config;

    private void Start()
    {
        StartCoroutine(DestroyAfterDelay(m_config.CannonBallDestroyDelay));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this);
    }

    private void Update()
    {
        this.transform.position += new Vector3(m_config.CannonBallSpeed * Time.deltaTime, 0f, 0f);
    }
}
