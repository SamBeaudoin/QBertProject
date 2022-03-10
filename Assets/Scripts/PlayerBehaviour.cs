using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private Animator m_Animator;
    private Transform m_Transform;
    private Rigidbody2D m_Rb;

    [SerializeField]
    public GameObject m_CurrentBlock;

    public int m_CurrentSortOrder;
    public bool m_CanJump = true;

    [SerializeField]
    public Vector3 m_Offest = new Vector3(0.0f, 0.1f, 0.0f);



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
        m_CurrentSortOrder = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Sorting Order update
        GetComponent<SpriteRenderer>().sortingOrder = m_CurrentSortOrder;


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

    // Collisions
    // Block Snapping for Snappy-ness
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "LevelBlocks")
        {
            if(collision.collider.GetComponent<BlockBehaviour>().m_CurrentLayer > m_CurrentSortOrder)
            {
                collision.collider.GetComponent<EdgeCollider2D>().enabled = false;
                return;
            }
            m_Transform.position = collision.transform.position + m_Offest;
            collision.gameObject.GetComponent<BlockBehaviour>().m_IsChanged = true;
            m_CurrentBlock = collision.gameObject;
        }
        if(collision.collider.tag == "Elevator")
        {
            // establish connection 
            this.transform.SetParent(collision.transform);
            collision.gameObject.GetComponent<ElevatorBehaviour>().m_PlayerCollision = true;
            m_CanJump = false;
        }
        if(collision.collider.tag == "RedBall" || collision.collider.tag == "GreenBall")
        {
            Destroy(collision.collider.gameObject);
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
