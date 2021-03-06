﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class UITypeWriter : MonoBehaviour
{
    [SerializeField] private float m_canSkipDelay = 0.5f;
    [SerializeField] private Text m_text;
    [SerializeField] private float m_delayBeforeStart;
    [SerializeField] private float m_delay = 0.125f;
    [SerializeField] private bool m_allowSkip;
    [SerializeField] private UnityEvent m_onTextComplete;

    private string m_story;
    private float m_accumulatedTime;

    private void Awake ()
    {
        m_story = m_text.text;
        m_text.text = string.Empty;

        StartCoroutine(PlayText());
    }

    private IEnumerator PlayText()
    {
        yield return new WaitForSeconds(m_delayBeforeStart);

        foreach (char c in m_story)
        {
            m_text.text += c;

            if (m_delay > 0f)
            {
                yield return new WaitForSeconds (m_delay);
            }
        }

        m_onTextComplete.Invoke();
    }

    public void Update()
    {
        m_accumulatedTime += Time.deltaTime;

        if (m_allowSkip && m_accumulatedTime > m_canSkipDelay && Input.GetMouseButtonDown(0))
        {
            m_delay = 0f;
        }
    }

}
