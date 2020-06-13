using UnityEngine;

public class SkyflyBehaviour : MonoBehaviour
{
    [SerializeField] private GameConfig m_config;

    private void Update()
    {
        this.transform.position -= new Vector3(m_config.SkyFlyMoveSpeed * Time.deltaTime, 0f, 0f);
    }
}
