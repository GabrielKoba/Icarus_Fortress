using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class CannonBallBehaviour : MonoBehaviour
{
    [SerializeField] private GameConfig m_config;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(DestroyAfterDelay(m_config.CannonBallDestroyDelay));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void Update()
    {
        this.transform.position += new Vector3(m_config.CannonBallSpeed * Time.deltaTime, 0f, 0f);
    }
}
