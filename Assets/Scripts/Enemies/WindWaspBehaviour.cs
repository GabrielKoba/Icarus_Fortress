using UnityEngine;

public class WindWaspBehaviour : MonoBehaviour
{
    [SerializeField] private GameConfig m_config;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        this.transform.position -= new Vector3(m_config.WindWaspMoveSpeed * Time.deltaTime, 0f, 0f);
    }
}
