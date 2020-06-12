using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameConfig m_gameConfig;
    [SerializeField] private CharacterController2D m_controller;
    [SerializeField] private Rigidbody2D m_rigidBody;
    [SerializeField] private GameObject m_heldCannonBall;

    private float m_horizontalMove = 0f;
    private bool m_jump = false;
    private bool m_canPickupCannonBall = false;
    private bool m_canDropOffCannonBall = false;

    private const string BALL_PILE_COLLIDER_TAG = "BallPile";
    private const string CANNON_COLLIDER_TAG = "Cannon";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(BALL_PILE_COLLIDER_TAG))
        {
            m_canPickupCannonBall = true;
            return;
        }

        if (other.CompareTag(CANNON_COLLIDER_TAG))
        {
            m_canDropOffCannonBall = true;
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(BALL_PILE_COLLIDER_TAG))
        {
            m_canPickupCannonBall = false;
        }

        if (other.CompareTag(BALL_PILE_COLLIDER_TAG))
        {
            m_canDropOffCannonBall = false;
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
        }

        if (Input.GetKeyDown(KeyCode.E) && m_canDropOffCannonBall)
        {
            m_heldCannonBall.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        m_controller.Move(m_horizontalMove * Time.fixedDeltaTime, false, m_jump);
        m_jump = false;
    }
}
