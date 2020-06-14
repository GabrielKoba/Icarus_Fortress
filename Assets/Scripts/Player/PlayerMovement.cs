using System;
using System.Collections;
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
    [SerializeField] private Sprite m_capacityFullBiggest;
    [SerializeField] private Sprite m_capacityEmpty;
    [SerializeField] private GameObject m_loadingBar;

    [SerializeField] private Sprite m_bigCannonBallSprite;
    [SerializeField] private Sprite m_defaultCannonBallSprite;

    [Header("Audio Settings")]
    [FMODUnity.EventRef][SerializeField]string jumpSFX;
    [FMODUnity.EventRef][SerializeField]string ballPickupSFX;
    [FMODUnity.EventRef][SerializeField]string ballLoadingSFX;

    public bool m_isControlledByTutorial = false;

    private float m_horizontalMove = 0f;
    private bool m_jump = false;

    private string m_ballPileRangeIdentifier;
    //private bool m_canPickupCannonBall = false;
    private bool m_isPickingUpCannonBall = false;

    private string m_cannonRangeIdentifier;

    private int m_numHeldCannonBalls = 0;
    private const int PLAYER_MAX_CANNONBALLS = 3;

    private bool m_hasTutJumped = false;
    private bool m_hasTutHitted = false;

    public static float PickingUpCooldownBig;

    private readonly HashSet<string> m_cannonTags = new HashSet<string>()
    {
        "CannonTop",
        "CannonMiddle",
        "CannonBottom"
    };

    private readonly HashSet<string> m_ballPileTags = new HashSet<string>()
    {
        "BallPile",
        "BallPileRed",
        "BallPileBlue"
    };

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_ballPileTags.Contains(other.tag))
        {
            m_ballPileRangeIdentifier = other.tag;
            return;
        }

        if (m_cannonTags.Contains(other.tag))
        {
            m_cannonRangeIdentifier = other.tag;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (m_ballPileTags.Contains(other.tag))
        {
            m_ballPileRangeIdentifier = string.Empty;
            return;
        }

        if (m_cannonTags.Contains(other.tag))
        {
            m_cannonRangeIdentifier = string.Empty;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (m_isControlledByTutorial && other.gameObject.name == "Platform1")
        {
            m_horizontalMove = 0f;
            StartCoroutine(AnimateShipUp());
            StartCoroutine(LoadSceneAfterDelay());
        }
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(4f);
        StartCoroutine(GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, "SampleScene"));
    }

    private IEnumerator AnimateShipUp()
    {
        var time = 0f;
        var ship = GameObject.FindGameObjectWithTag("AirShip");

        yield return new WaitForSeconds(1f);
        while (time < 10f)
        {
            time += Time.deltaTime;
            ship.transform.localPosition += new Vector3(0f, 2.5f* Time.deltaTime, 0f);

            yield return null;
        }
    }

    private void Start()
    {
        PickingUpCooldownBig = m_gameConfig.PlayerPickingUpBiggestCannonBallCoolDown;

        m_controller.m_JumpForce = m_gameConfig.PlayerJumpForce;
        m_rigidBody.gravityScale = m_gameConfig.PlayerGravity;
    }

    public void MoveForTutorial()
    {
        m_isControlledByTutorial = true;
        StartCoroutine(AutoMoveCamera());
        StartCoroutine(AutoMovePlayer());
    }

    private IEnumerator AutoMoveCamera()
    {

        var camera = GameObject.FindGameObjectWithTag("FakeCamera");

        while (camera.transform.localPosition.x > -7f)
        {
            camera.transform.localPosition += new Vector3(-2.5f * Time.deltaTime, 0f, 0f);
            yield return null;
        }
    }

    private IEnumerator AutoMovePlayer()
    {
        m_horizontalMove = -80f;
        while (transform.localPosition.x > -14f)
        {
            yield return null;
        }

        m_jump = true;
    }

    private IEnumerator TryPickingUpCannonBall(bool isBigCannonBall)
    {
        var time = 0f;

        var cooldown = isBigCannonBall ? PickingUpCooldownBig : m_gameConfig.PlayerPickingUpCannonBallCoolDown;

        while (time < cooldown)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                m_isPickingUpCannonBall = false;
                m_loadingBar.transform.localScale = new Vector3(m_loadingBar.transform.localScale.x, 0f, m_loadingBar.transform.localScale.z);
                yield break;
            }

            var factor = time / cooldown;

            time += Time.deltaTime;

            m_loadingBar.transform.localScale = new Vector3(m_loadingBar.transform.localScale.x, factor, m_loadingBar.transform.localScale.z);
            yield return null;
        }

        // Success!
        m_loadingBar.transform.localScale = new Vector3(m_loadingBar.transform.localScale.x, 0f, m_loadingBar.transform.localScale.z);

        FMODUnity.RuntimeManager.PlayOneShot(ballPickupSFX, transform.position);


        if (CannonBehaviour.s_cannonsFillInstantly && isBigCannonBall)
        {
            m_numHeldCannonBalls = 2;
            foreach (var capacityIndicator in m_capacityIndicators)
            {
                capacityIndicator.sprite = m_capacityFullBiggest;
            }
        }

        if (m_numHeldCannonBalls < PLAYER_MAX_CANNONBALLS)
        {
            m_capacityIndicators[m_numHeldCannonBalls].sprite = isBigCannonBall ? m_capacityFullBiggest : m_capacityFull;
            m_numHeldCannonBalls++;
        }

        if (isBigCannonBall)
        {
            m_heldCannonBall.GetComponent<SpriteRenderer>().sprite = m_bigCannonBallSprite;
        }
        else
        {
            m_heldCannonBall.GetComponent<SpriteRenderer>().sprite = m_defaultCannonBallSprite;
        }

        m_heldCannonBall.SetActive(true);
        m_isPickingUpCannonBall = false;
    }

    private void Update()
    {
        if (m_isControlledByTutorial)
        {
            return;
        }

        m_horizontalMove = Input.GetAxisRaw("Horizontal") * m_gameConfig.PlayerMoveSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            //Sound
            FMODUnity.RuntimeManager.PlayOneShot(jumpSFX, transform.position);
            
            m_jump = true;
        }

        // Picking Up!
        if (Input.GetKey(KeyCode.E) && !string.IsNullOrEmpty(m_ballPileRangeIdentifier) && !m_isPickingUpCannonBall && m_numHeldCannonBalls < PLAYER_MAX_CANNONBALLS)
        {
            m_isPickingUpCannonBall = true;
            StartCoroutine(TryPickingUpCannonBall(m_ballPileRangeIdentifier == "BallPileRed"));
        }

        // Loading
        if (Input.GetKeyDown(KeyCode.E) && !string.IsNullOrEmpty(m_cannonRangeIdentifier) && m_heldCannonBall.activeSelf && !GameObject.FindGameObjectWithTag(m_cannonRangeIdentifier).GetComponent<CannonBehaviour>().IsFullyLoaded)
        {
            //Sound
            FMODUnity.RuntimeManager.PlayOneShot(ballLoadingSFX, transform.position);


            m_numHeldCannonBalls--;
            Debug.Log($"Loading! Num held balls {m_numHeldCannonBalls}");
            m_cannonLoaded.Raise($"{m_cannonRangeIdentifier},{m_capacityIndicators[m_numHeldCannonBalls].sprite.name}");

            m_capacityIndicators[m_numHeldCannonBalls].sprite = m_capacityEmpty;
            if (m_numHeldCannonBalls == 0)
            {
                m_heldCannonBall.SetActive(false);
            }
            else
            {
                if (m_capacityIndicators[m_numHeldCannonBalls - 1].sprite.name == "Cannon_Capacity_3" && !CannonBehaviour.s_cannonsFillInstantly)
                {
                    m_heldCannonBall.GetComponent<SpriteRenderer>().sprite = m_bigCannonBallSprite;
                }
                else if (m_capacityIndicators[m_numHeldCannonBalls - 1].sprite.name == "Cannon_Capacity_3")
                {
                    foreach (var indicator in m_capacityIndicators)
                    {
                        indicator.sprite = m_capacityEmpty;
                    }

                    m_heldCannonBall.SetActive(false);
                    m_numHeldCannonBalls = 0;
                }
                else
                {
                    m_heldCannonBall.GetComponent<SpriteRenderer>().sprite = m_defaultCannonBallSprite;
                }
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
