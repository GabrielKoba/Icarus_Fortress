using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Transform m_boundaryRight;
    [SerializeField] private Transform m_boundaryLeft;

    [SerializeField] private SceneFader[] m_stuffToFadeOutAfterText;
    [SerializeField] private string m_sceneToLoad;
    [SerializeField] private SceneFader m_fader;
    [SerializeField] private SceneFader m_pressContinueFader;

    [SerializeField] private Text m_greatText;
    [SerializeField] private Text m_text;
    [SerializeField] private Text m_tutText;

    [SerializeField] private Text m_pickupText;
    [SerializeField] private Text m_fireText;

    [SerializeField] private GameObject m_indicators;

    private bool m_textIsComplete = false;

    private int m_shotsFired;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(m_fader.FadeAndLoadScene(SceneFader.FadeDirection.In, sceneName));
    }

    public void OnTextComplete()
    {
        StartCoroutine(FadeOutStuffAfterDelay());
        //StartCoroutine(DelayCanProceed());
    }

    public void OnShotFired()
    {
        if (m_shotsFired > 1)
        {
            Debug.Log("Alright!, lets proceeed!");

            StartCoroutine(FadeOutRoutine(1f, m_tutText, false));

        }

        m_shotsFired++;
    }

    private IEnumerator FadeOutStuffAfterDelay()
    {
        m_boundaryRight.transform.localPosition = new Vector3(9.1f, m_boundaryRight.transform.localPosition.y, m_boundaryRight.localPosition.z);
        m_boundaryLeft.transform.localPosition = new Vector3(-9.1f, m_boundaryLeft.transform.localPosition.y, m_boundaryLeft.transform.localPosition.z);

        yield return new WaitForSeconds(2.5f);

        m_indicators.SetActive(true);

        StartCoroutine(FadeOutRoutine(2.5f, m_text, true));

        foreach (var stuff in m_stuffToFadeOutAfterText)
        {
            stuff.FadeImage(1);
        }
    }

    public void OnGreatTextComplete()
    {
        // foreach (var stuff in m_stuffToFadeOutAfterText)
        // {
        //     stuff.FadeImage(0);
        // }


        m_stuffToFadeOutAfterText[0].FadeImage(0);

        StartCoroutine(ProceedAfterDelay());
    }

    private IEnumerator ProceedAfterDelay()
    {

        StartCoroutine(FadeOutRoutineWithoutCallback(1f, m_greatText));
        StartCoroutine(FadeOutRoutineWithoutCallback(1f, m_pickupText));
        StartCoroutine(FadeOutRoutineWithoutCallback(1f, m_fireText));

        yield return new WaitForSeconds(2.5f);

        m_boundaryLeft.gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().MoveForTutorial();
    }

    private IEnumerator FadeOutRoutineWithoutCallback(float time, Text text)
    {
        Color originalColor = text.color;
        for (float t = 0.01f; t < time; t += Time.deltaTime)
        {
            text.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t/time));
            yield return null;
        }
    }

    private IEnumerator FadeOutRoutine(float time, Text text, bool tutTextCallback)
    {
        Color originalColor = text.color;
        for (float t = 0.01f; t < time; t += Time.deltaTime)
        {
            text.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t/time));
            yield return null;
        }

        if (tutTextCallback)
        {
            m_tutText.gameObject.SetActive(true);
        }
        else
        {
            m_greatText.gameObject.SetActive(true);
        }

    }

    // private IEnumerator DelayCanProceed()
    // {
    //    // yield return new WaitForSeconds(2.5f);
    //     m_textIsComplete = true;
    // }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y)) {
            LoadScene(m_sceneToLoad);
        }

        // if (m_textIsComplete)
        // {
        //     m_pressContinueFader.FadeImage(1);
        // }
        //
        // if ( (Input.GetKey(KeyCode.Return) || Input.GetMouseButtonDown(0)) && m_textIsComplete)
        // {
        //     LoadScene(m_sceneToLoad);
        // }
    }
}
