using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ProgressBarBehaviour : MonoBehaviour
{
    [SerializeField] private Transform m_startPos;
    [SerializeField] private Transform m_endPos;
    [SerializeField] private GameConfig m_config;

    private Vector3 m_startVector;
    private float m_time;
    // Start is called before the first frame update
    void Start()
    {
        m_startVector = m_startPos.transform.localPosition;
        StartCoroutine(AnimateTowardsBoss());
    }

    private IEnumerator AnimateTowardsBoss()
    {
        var distance = Mathf.Abs(m_startPos.localPosition.x - m_endPos.localPosition.x);

        Debug.Log("TOTAL dist" + distance);

        while (m_startPos.localPosition.x <= m_endPos.localPosition.x)
        {
            m_time += Time.deltaTime;

            var factor = m_time / m_config.TimeBeforeBossSpawn;
            var currentDistance = distance * factor;

            m_startPos.transform.localPosition = new Vector3(m_startVector.x + currentDistance, m_startPos.transform.localPosition.y, m_startPos.transform.localPosition.z);
            yield return null;
        }
    }
}
