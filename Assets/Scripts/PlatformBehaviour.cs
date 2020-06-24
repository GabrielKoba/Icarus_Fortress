using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    [SerializeField] private PlatformEffector2D m_effector;

    private bool m_isDropping = false;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.S) && !m_isDropping || Input.GetKey(KeyCode.DownArrow) && !m_isDropping)
        {
            m_isDropping = true;
            m_effector.rotationalOffset = 180f;
            StartCoroutine(RotateBackEffectorAfterDelay(0.15f));
        }

        if (Input.GetKeyUp(KeyCode.S) && Mathf.Approximately(m_effector.rotationalOffset, 0f) || Input.GetKeyUp(KeyCode.DownArrow) && Mathf.Approximately(m_effector.rotationalOffset, 0f))
        {
            m_isDropping = false;
        }
    }


    private IEnumerator RotateBackEffectorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        m_effector.rotationalOffset = 0f;
    }

}
