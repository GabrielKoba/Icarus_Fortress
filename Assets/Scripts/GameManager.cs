// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class GameManager : MonoBehaviour
// {
//     [SerializeField] private GameConfig m_config;
//
//     private float m_timeBetweenDifficultyIncrease;
//
//     // Start is called before the first frame update
//     void Start()
//     {
//         m_timeBetweenDifficultyIncrease = m_config.TimeBetweenDifficultyIncrease;
//         StartCoroutine(IncreaseDifficultyOverTime());
//     }
//
//     private IEnumerator IncreaseDifficultyOverTime()
//     {
//         yield return new WaitForSeconds(m_timeBetweenDifficultyIncrease);
//
//         m_config.MinDelay--;
//         m_config.MaxDelay--;
//
//         Debug.Log($"<color=green>[Increasing difficulty] Min: {m_config.MinDelay} Max: {m_config.MaxDelay}</color>", this);
//     }
// }
