using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CannonBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject m_cannonBallSpawnPosition;
    [SerializeField] private GameObject m_cannonBallPrefab;
    [SerializeField] private GameObject m_cannonBallBigPrefab;

    [SerializeField] private SpriteRenderer[] m_capacityIndicators;
    [SerializeField] private Sprite m_capacityFull;
    [SerializeField] private Sprite m_capacityEmpty;
    [SerializeField] private Sprite m_capacityBig;

    private int m_cannonInventory = 0;

    [Header("Audio Settings")]
    [FMODUnity.EventRef][SerializeField]string fireSFX;

    [Header("Animation Settings")]

    [SerializeField] GameObject smokeEffectPrefab;
    private Animator m_Animator;
    private Animator m_smokeAnimator;

    public bool IsFullyLoaded => m_cannonInventory == CANNON_MAX_CAPACITY;

    private const int CANNON_MAX_CAPACITY = 3;

    void Awake() {
        m_Animator = gameObject.GetComponent<Animator>();
        m_smokeAnimator = smokeEffectPrefab.GetComponent<Animator>();
    }

    public void OnCannonLoaded(string cannonId)
    {
        var parameters = cannonId.Split(',').ToList<string>();

        if (parameters[0] != this.tag)
        {
            return;
        }

        if (m_cannonInventory < CANNON_MAX_CAPACITY)
        {
            bool loadBigSprite = parameters[1] == "Cannon_Capacity_3";

            m_capacityIndicators[m_cannonInventory].sprite = loadBigSprite ? m_capacityBig : m_capacityFull;
            m_cannonInventory++;
        }
    }

    public void OnCannonFired(string cannonId)
    {
        if (cannonId != this.tag || m_cannonInventory == 0)
        {
            return;
        }

        m_cannonInventory--;

        bool isBigBall = m_capacityIndicators[m_cannonInventory].sprite.name == "Cannon_Capacity_3";
        var prefab = isBigBall ? m_cannonBallBigPrefab : m_cannonBallPrefab;
        Instantiate(prefab, m_cannonBallSpawnPosition.transform);

        if (isBigBall)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>().shakeDuration = 0.15f;

        }

        m_capacityIndicators[m_cannonInventory].sprite = m_capacityEmpty;

        //Sound
        FMODUnity.RuntimeManager.PlayOneShot(fireSFX, transform.position);

        //Animation
        m_Animator.SetTrigger("Fire");
        m_smokeAnimator.SetTrigger("Fire");

        Debug.Log($"{this.tag} fired!");
    }
}
