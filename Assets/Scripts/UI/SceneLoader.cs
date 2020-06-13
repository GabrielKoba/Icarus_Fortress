using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string m_sceneToLoad;

    private bool m_textIsComplete = false;

    private void LoadScene(string sceneName)
    {
        StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, sceneName));
    }

    public void OnTextComplete()
    {
        m_textIsComplete = true;
    }

    public void Update()
    {
        if (m_textIsComplete && Input.GetMouseButtonDown(0))
        {
            LoadScene(m_sceneToLoad);
        }
    }
}
