using UnityEngine;

public class CloudBeetleBehaviour : MonoBehaviour
{
    [SerializeField] private GameConfig m_config;
    [SerializeField] private GameEventStringParam m_enemyHitPlayer;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag != "Enemy")
        {
            Destroy(gameObject);
        }

        if (other.collider.tag == "AirShip")
        {
            m_enemyHitPlayer.Raise(string.Empty);
        }
    }

    private void Update()
    {
        this.transform.position -= new Vector3(m_config.CloudBeetleMoveSpeed * Time.deltaTime, 0f, 0f);
    }
}
