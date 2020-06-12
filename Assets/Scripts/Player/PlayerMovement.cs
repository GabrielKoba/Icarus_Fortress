using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController2D m_controller;
    [SerializeField] private float m_runSpeed = 40f;

    private float m_horizontalMove = 0f;
    private bool m_jump = false;

    // Update is called once per frame
    private void Update()
    {
        m_horizontalMove = Input.GetAxisRaw("Horizontal") * m_runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            m_jump = true;
        }
    }

    private void FixedUpdate()
    {
        m_controller.Move(m_horizontalMove * Time.fixedDeltaTime, false, m_jump);
        m_jump = false;
    }
}
