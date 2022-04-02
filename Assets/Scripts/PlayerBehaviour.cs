using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    private Animator m_Animator;
    private Transform m_Transform;
    private Rigidbody2D m_Rb;

    [SerializeField]
    public GameObject m_CurrentBlock;

    [SerializeField]
    public GameObject m_PlayerLives_1;
    [SerializeField]
    public GameObject m_PlayerLives_2;

    private int m_Lives = 2;
    private Vector3 m_SpawnPosition1;
    private Vector3 m_SpawnPosition2;

    [SerializeField]
    public GameObject m_PGameOverMenuUI;
    [SerializeField]
    public Text m_ScoreText;

    public GameObject[] m_AllLevelBlocks;
    public int m_Score = 0;

    public int m_CurrentSortOrder;
    public bool m_CanJump = true;

    [SerializeField]
    public Vector3 m_Offest = new Vector3(0.0f, 0.1f, 0.0f);

    private bool m_GameCompleted = false;

    // Velocity Experimentation -Complete-
    //[SerializeField]
    //[Range(0.0f, 5.0f)]
    //private float m_XStrength = -0.2f;

    // Velocity Experimentation -Complete-
    //[SerializeField]
    //[Range(0.0f, 5.0f)]
    //private float m_YStrength = 1.75f;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Transform = this.transform;
        m_Rb = GetComponent<Rigidbody2D>();
        m_SpawnPosition1 = m_Rb.position;
        m_CurrentSortOrder = 1;
        m_AllLevelBlocks = GameObject.FindGameObjectsWithTag("LevelBlocks");
    }

    // Update is called once per frame
    void Update()
    {
        // Game Completed Check
        if(m_GameCompleted)
        {
            Debug.Log("YOU WIN! Huzzah!!");
        }

        // Sorting Order update
        GetComponent<SpriteRenderer>().sortingOrder = m_CurrentSortOrder;

        // Check if we have fallen
        KillFloorCheck();

        // Score update
        if(m_Score > 0)
        {
            m_ScoreText.text = "" + m_Score;
        }

        // Key Bindings
        if (Mathf.Abs(m_Rb.velocity.y) <= 0.01f && m_CanJump)
        {
            // If not jumping/falling

            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                m_Animator.SetBool("NK1_ButtonPressed", true);
            }
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                m_Animator.SetBool("NK3_ButtonPressed", true);
            }
            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                m_Animator.SetBool("NK7_ButtonPressed", true);
            }
            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                m_Animator.SetBool("NK9_ButtonPressed", true);
            }

            // Bottom Left
            if (Input.GetKeyUp(KeyCode.Keypad1))
            {
                m_Animator.SetBool("NK1_ButtonPressed", false);
                JumpDownLeft();
            }

            // Bottom Right
            if (Input.GetKeyUp(KeyCode.Keypad3))
            {
                m_Animator.SetBool("NK3_ButtonPressed", false);
                JumpDownRight();
            }

            // Top Left
            if (Input.GetKeyUp(KeyCode.Keypad7))
            {
                m_Animator.SetBool("NK7_ButtonPressed", false);
                JumpUpLeft();
            }

            // Top Right
            if (Input.GetKeyUp(KeyCode.Keypad9))
            {
                m_Animator.SetBool("NK9_ButtonPressed", false);
                JumpUpRight();
            }

        }
        
    }

    void GameCompletedCheck()
    {
        bool AllBlocksSwitched = false;
        foreach (GameObject block in m_AllLevelBlocks)
        {
            if(block.GetComponent<BlockBehaviour>().m_IsChanged)
            {
                AllBlocksSwitched = true;
            }
            else
            {
                AllBlocksSwitched = false;
                break;
            }
        }
        if (AllBlocksSwitched)
            m_GameCompleted = true;
    }

    private void KillFloorCheck()
    {
        if(m_Rb.position.y <= -0.75f)
        {
            m_CurrentSortOrder = 1;
            GetComponent<BoxCollider2D>().enabled = true;
            m_Rb.velocity = Vector3.zero;
            m_Rb.position = m_SpawnPosition1;
            m_Lives--;
            PlayerLivesCheck();
        }
    }

    private void PlayerLivesCheck()
    {
        if(m_Lives < 2)
        {
            m_PlayerLives_2.gameObject.SetActive(false);
        }
        if(m_Lives < 1)
        {
            m_PlayerLives_1.gameObject.SetActive(false);
        }
        if(m_Lives < 0)
        {
            // Game Over Code
            m_PGameOverMenuUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    // Collisions
    // Block Snapping for Snappy-ness
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "LevelBlocks")
        {
            if(collision.collider.GetComponent<BlockBehaviour>().m_CurrentLayer > m_CurrentSortOrder)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                return;
            }
            m_Transform.position = collision.transform.position + m_Offest;
            if(!collision.gameObject.GetComponent<BlockBehaviour>().m_IsChanged)
            {
                m_Score += 25;
                collision.gameObject.GetComponent<BlockBehaviour>().m_IsChanged = true;
                GameCompletedCheck();
            }

            m_CurrentBlock = collision.gameObject;
            m_SpawnPosition2 = m_CurrentBlock.gameObject.transform.position + m_Offest;
        }
        if(collision.collider.tag == "Elevator")
        {
            // establish connection 
            this.transform.SetParent(collision.transform);
            collision.gameObject.GetComponent<ElevatorBehaviour>().m_PlayerCollision = true;
            m_CurrentBlock = collision.gameObject;
            m_CanJump = false;
        }
        if(collision.collider.tag == "GreenBall")
        {
            m_Score += 100;
            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.tag == "RedBall")
        {
            m_Lives--;
            PlayerLivesCheck();
            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.tag == "Coily")
        {
            m_Lives--;
            PlayerLivesCheck();
            Destroy(collision.collider.gameObject);
            SpawnerBehaviour.Instance.m_CoilySpawned = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Remove connection to elevator
        if (collision.collider.tag == "Elevator")
        {
            this.transform.SetParent(null);
            m_CanJump = true;
        }
    }

    // Jumping Velocities
    public void JumpUpLeft()
    {
        Vector2 Direction = new Vector2(-0.32f, 1.75f);
        m_Rb.AddForce(Direction, ForceMode2D.Impulse);
        m_CurrentSortOrder -= 2;
    }

    public void JumpUpRight()
    {
        Vector2 Direction = new Vector2(0.32f, 1.75f);
        m_Rb.AddForce(Direction, ForceMode2D.Impulse);
        m_CurrentSortOrder -= 2;
    }

    public void JumpDownLeft()
    {
        Vector2 Direction = new Vector2(-0.34f, 0.75f);
        m_Rb.AddForce(Direction, ForceMode2D.Impulse);
        m_CurrentSortOrder += 2;
    }

    public void JumpDownRight()
    {
        Vector2 Direction = new Vector2(0.34f, 0.75f);
        m_Rb.AddForce(Direction, ForceMode2D.Impulse);
        m_CurrentSortOrder += 2;
    }
}
