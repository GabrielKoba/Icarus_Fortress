using UnityEngine;

public class CloudBeetleBehaviour : MonoBehaviour
{
    [SerializeField] private GameConfig m_config;

    private void Update()
    {
        this.transform.position -= new Vector3(m_config.CloudBeetleMoveSpeed * Time.deltaTime, 0f, 0f);
    }
}
