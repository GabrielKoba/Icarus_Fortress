using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLivesHandler : MonoBehaviour
{
    [SerializeField] private GameConfig m_config;
    [SerializeField] private Text m_debugText;

    private int m_currentLives;

    private void Start()
    {
        m_currentLives = m_config.PlayerStartingLives;
        m_debugText.text = $"Player Lives: {m_currentLives}";
    }

    public void OnBulletHitPlayer(string data)
    {
        m_currentLives--;
        m_debugText.text = $"Player Lives: {m_currentLives}";

        if (m_currentLives == 0)
        {
            m_debugText.text = "Death!";
        }
    }
}
