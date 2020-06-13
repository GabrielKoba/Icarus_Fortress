using UnityEngine;

public class CannonBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject m_cannonBallSpawnPosition;
    [SerializeField] private GameObject m_cannonBallPrefab;

    private int m_cannonInventory = 0;

    [Header("Audio Settings")]
    [FMODUnity.EventRef][SerializeField]string fireSFX;

    [Header("Animation Settings")]

    [SerializeField] GameObject smokeEffectPrefab;
    private Animator m_Animator;
    private Animator m_smokeAnimator;

    void Awake() {
        m_Animator = gameObject.GetComponent<Animator>();
        m_smokeAnimator = smokeEffectPrefab.GetComponent<Animator>();
    }

    public void OnCannonLoaded(string cannonId)
    {
        if (cannonId != this.tag)
        {
            return;
        }

        m_cannonInventory++;
    }

    public void OnCannonFired(string cannonId)
    {
        if (cannonId != this.tag || m_cannonInventory == 0)
        {
            return;
        }

        Instantiate(m_cannonBallPrefab, m_cannonBallSpawnPosition.transform);
        m_cannonInventory--;

        //Sound
        FMODUnity.RuntimeManager.PlayOneShot(fireSFX, transform.position);

        //Animation
        m_Animator.SetTrigger("Fire");
        m_smokeAnimator.SetTrigger("Fire");

        Debug.Log($"{this.tag} fired!");
    }
}
