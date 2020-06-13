using System.Collections;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string m_sceneToLoad;
    [SerializeField] private SceneFader m_fader;
    [SerializeField] private SceneFader m_pressContinueFader;

    private bool m_textIsComplete = false;

    private void LoadScene(string sceneName)
    {
        StartCoroutine(m_fader.FadeAndLoadScene(SceneFader.FadeDirection.In, sceneName));
    }

    public void OnTextComplete()
    {
        StartCoroutine(DelayCanProceed());
    }

    private IEnumerator DelayCanProceed()
    {
        yield return new WaitForSeconds(2.5f);
        m_textIsComplete = true;
    }

    public void Update()
    {
        if (m_textIsComplete)
        {
            m_pressContinueFader.FadeImage(1);
        }

        if ( (Input.GetKey(KeyCode.Return) || Input.GetMouseButtonDown(0)) && m_textIsComplete)
        {
            LoadScene(m_sceneToLoad);
        }
    }
}
