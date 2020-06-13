using UnityEngine;

public class WindWaspBehaviour : MonoBehaviour
{
    [SerializeField] private GameConfig m_config;
    [SerializeField] private GameEventStringParam m_enemyHitPlayer;

    [Header("Audio Settings")]
    [FMODUnity.EventRef][SerializeField]string deathSFX;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag != "Enemy")
        {
            //Sound
            FMODUnity.RuntimeManager.PlayOneShot(deathSFX, transform.position);

            Destroy(gameObject);
        }

        if (other.collider.tag == "AirShip")
        {
            m_enemyHitPlayer.Raise(string.Empty);
        }
    }

    private void Update()
    {
        this.transform.position -= new Vector3(m_config.WindWaspMoveSpeed * Time.deltaTime, 0f, 0f);
    }
}
