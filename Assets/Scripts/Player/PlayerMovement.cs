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

    private float m_horizontalMove = 0f;
    private bool m_jump = false;
    private bool m_canPickupCannonBall = false;

    private string m_cannonRangeIdentifier;

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
            m_jump = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && m_canPickupCannonBall)
        {
            m_heldCannonBall.SetActive(true);
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && !string.IsNullOrEmpty(m_cannonRangeIdentifier) && m_heldCannonBall.activeSelf)
        {
            m_cannonLoaded.Raise(m_cannonRangeIdentifier);
            m_heldCannonBall.SetActive(false);
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && !string.IsNullOrEmpty(m_cannonRangeIdentifier))
        {
            m_cannonFired.Raise(m_cannonRangeIdentifier);
        }
    }

    private void FixedUpdate()
    {
        m_controller.Move(m_horizontalMove * Time.fixedDeltaTime, false, m_jump);
        m_jump = false;
    }
}
