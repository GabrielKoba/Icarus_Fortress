using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameConfig m_gameConfig;
    [SerializeField] private CharacterController2D m_controller;
    [SerializeField] private Rigidbody2D m_rigidBody;
    [SerializeField] private GameObject m_heldCannonBall;

    [SerializeField] private GameEventStringParam m_cannonLoaded;
    [SerializeField] private GameEventStringParam m_cannonFired;
    [SerializeField] private SpriteRenderer[] m_capacityIndicators;
    [SerializeField] private Sprite m_capacityFull;
    [SerializeField] private Sprite m_capacityEmpty;

    [Header("Audio Settings")]
    [FMODUnity.EventRef][SerializeField]string jumpSFX;
    [FMODUnity.EventRef][SerializeField]string ballPickupSFX;
    [FMODUnity.EventRef][SerializeField]string ballLoadingSFX;

    private float m_horizontalMove = 0f;
    private bool m_jump = false;
    private bool m_canPickupCannonBall = false;

    private string m_cannonRangeIdentifier;

    private int m_numHeldCannonBalls = 0;
    private const int PLAYER_MAX_CANNONBALLS = 3;
    private const string BALL_PILE_COLLIDER_TAG = "BallPile";

    private readonly HashSet<string> m_cannonTags = new HashSet<string>()
    {
        "CannonTop",
        "CannonMiddle",
        "CannonBottom"
    };

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(BALL_PILE_COLLIDER_TAG))
        {
            m_canPickupCannonBall = true;
            return;
        }

        if (m_cannonTags.Contains(other.tag))
        {
            m_cannonRangeIdentifier = other.tag;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(BALL_PILE_COLLIDER_TAG))
        {
            m_canPickupCannonBall = false;
        }

        if (m_cannonTags.Contains(other.tag))
        {
            m_cannonRangeIdentifier = string.Empty;
        }
    }

    private void Start()
    {
        m_controller.m_JumpForce = m_gameConfig.PlayerJumpForce;
        m_rigidBody.gravityScale = m_gameConfig.PlayerGravity;
    }

    private void Update()
    {
        m_horizontalMove = Input.GetAxisRaw("Horizontal") * m_gameConfig.PlayerMoveSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            //Sound
            FMODUnity.RuntimeManager.PlayOneShot(jumpSFX, transform.position);
            
            m_jump = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && m_canPickupCannonBall)
        {
            //Sound
            FMODUnity.RuntimeManager.PlayOneShot(ballPickupSFX, transform.position);

            if (m_numHeldCannonBalls < PLAYER_MAX_CANNONBALLS)
            {
                m_capacityIndicators[m_numHeldCannonBalls].sprite = m_capacityFull;
                m_numHeldCannonBalls++;
            }
            m_heldCannonBall.SetActive(true);
            return;
        }


        // Loading
        if (Input.GetKeyDown(KeyCode.E) && !string.IsNullOrEmpty(m_cannonRangeIdentifier) && m_heldCannonBall.activeSelf && !GameObject.FindGameObjectWithTag(m_cannonRangeIdentifier).GetComponent<CannonBehaviour>().IsFullyLoaded)
        {
            //Sound
            FMODUnity.RuntimeManager.PlayOneShot(ballLoadingSFX, transform.position);

            Debug.Log($"Loading! Num held balls {m_numHeldCannonBalls}");
            m_cannonLoaded.Raise(m_cannonRangeIdentifier);

            m_numHeldCannonBalls--;
            m_capacityIndicators[m_numHeldCannonBalls].sprite = m_capacityEmpty;
            if (m_numHeldCannonBalls == 0)
            {
                m_heldCannonBall.SetActive(false);
            }

            return;
        }

        // Firing
        if (Input.GetKeyDown(KeyCode.E) && !string.IsNullOrEmpty(m_cannonRangeIdentifier))
        {
            Debug.Log($"Firing! Num held balls {m_numHeldCannonBalls}");
            m_cannonFired.Raise(m_cannonRangeIdentifier);
        }
    }

    private void FixedUpdate()
    {
        m_controller.Move(m_horizontalMove * Time.fixedDeltaTime, false, m_jump);
        m_jump = false;
    }
}
