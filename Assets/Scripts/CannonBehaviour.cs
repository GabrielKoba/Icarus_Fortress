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
    [SerializeField] private GameConfig m_config;

    private int m_cannonInventory = 0;

    [Header("Audio Settings")]
    [FMODUnity.EventRef][SerializeField]string fireSFX;

    [Header("Animation Settings")]

    [SerializeField] GameObject smokeEffectPrefab;
    private Animator m_Animator;
    private Animator m_smokeAnimator;

    public static bool s_cannonsFillInstantly = true;

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


        bool loadBigSprite = parameters[1] == "Cannon_Capacity_3";
        // Fill up all slots with the big one
        if (loadBigSprite && s_cannonsFillInstantly)
        {
            m_cannonInventory = 0;
            for (int i = 0; i < CANNON_MAX_CAPACITY; ++i)
            {
                m_capacityIndicators[m_cannonInventory].sprite = loadBigSprite ? m_capacityBig : m_capacityFull;
                m_cannonInventory++;
            }
            return;
        }

        if (m_cannonInventory < CANNON_MAX_CAPACITY)
        {
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

        if (isBigBall && s_cannonsFillInstantly)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>().shakeDuration = 0.15f;
            m_cannonInventory = 0;
            foreach (var indicator in m_capacityIndicators)
            {
                indicator.sprite = m_capacityEmpty;
            }
        }
        else
        {
            m_capacityIndicators[m_cannonInventory].sprite = m_capacityEmpty;
        }

        //Sound
        FMODUnity.RuntimeManager.PlayOneShot(fireSFX, transform.position);

        //Animation
        m_Animator.SetTrigger("Fire");
        m_smokeAnimator.SetTrigger("Fire");

        Debug.Log($"{this.tag} fired!");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            s_cannonsFillInstantly = !s_cannonsFillInstantly;

            if (s_cannonsFillInstantly)
            {
                PlayerMovement.PickingUpCooldownBig = m_config.PlayerPickingUpBiggestCannonBallCoolDown;
            }
            else
            {
                PlayerMovement.PickingUpCooldownBig = 3f;
            }
        }
    }
}
