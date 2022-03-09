using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoilySnakeBehaviour : MonoBehaviour
{
    private int m_CurrentSortOrder;
    private Rigidbody2D m_Rb;
    private Animator m_Animator;
    private Transform m_Transform;

    private float m_JumpInterval = 1.5f;

    public Vector3 m_Offset = new Vector3(0.0f, 0.2f, 0.0f);

    [SerializeField]
    public GameObject m_Player;

    [SerializeField]
    public GameObject m_CurrentBlock;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Transform = this.transform;
        this.transform.SetParent(null);
        m_Rb = GetComponent<Rigidbody2D>();
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_CurrentSortOrder = 13;
    }

    // Update is called once per frame
    void Update()
    {
        // Sorting Order update
        GetComponent<SpriteRenderer>().sortingOrder = m_CurrentSortOrder;

        // Debug Reset
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_Transform.position = m_CurrentBlock.transform.position + m_Offset;
        }
    }

    // Collisions
    // Block Snapping for Snappy-ness
    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_Transform.position = collision.transform.position + m_Offset;

        if (collision.collider.tag == "LevelBlocks")
        {
            m_Animator.SetBool("IsGrounded", true);
            m_Transform.position = collision.transform.position + m_Offset;
            m_Rb.velocity = Vector2.zero;
            m_CurrentBlock = collision.gameObject;
            JumpDecision();
        }
    }

    private void JumpDecision()
    {
        bool JumpRandom = (Random.Range(0.0f, 1.0f) >= 0.5f);
        bool PlayerIsLeft = false;
        bool PlayerIsRight = false;
        bool PlayerIsAbove = false;
        bool PlayerIsBelow = false;

        GameObject PlayerBlock = m_Player.GetComponent<PlayerBehaviour>().m_CurrentBlock;

        if(m_CurrentBlock.transform.position.x > PlayerBlock.transform.position.x)  // Player is to the Left
        {
            PlayerIsLeft = true;
        }
        if (m_CurrentBlock.transform.position.x < PlayerBlock.transform.position.x)  // Player is to the Right
        {
            PlayerIsRight = true;
        }
        if (m_CurrentBlock.transform.position.y > PlayerBlock.transform.position.y)  // Player is Below
        {
            PlayerIsBelow = true;
        }
        if (m_CurrentBlock.transform.position.y < PlayerBlock.transform.position.y)  // Player is Above
        {
            PlayerIsAbove = true;
        }

        if(PlayerIsAbove && !(PlayerIsLeft || PlayerIsRight))   // Player is Straight Above
        {
            if (JumpRandom)
                JumpUpLeft();
            else
                JumpUpRight();
            return;
        }

        if (PlayerIsBelow && !(PlayerIsLeft || PlayerIsRight))   // Player is Straight Below
        {
            if (JumpRandom)
                JumpDownLeft();
            else
                JumpDownRight();
            return;
        }

        if (PlayerIsRight && !(PlayerIsAbove || PlayerIsBelow))   // Player is Straight Right
        {
            if (JumpRandom)
                JumpDownRight();
            else
                JumpUpRight();
            return;
        }

        if (PlayerIsLeft && !(PlayerIsAbove || PlayerIsBelow))   // Player is Straight Left
        {
            if (JumpRandom)
                JumpDownLeft();
            else
                JumpUpLeft();
            return;
        }

        if (PlayerIsLeft && PlayerIsAbove)
            JumpUpLeft();
        else if (PlayerIsLeft && PlayerIsBelow)
            JumpDownLeft();
        else if (PlayerIsRight && PlayerIsAbove)
            JumpUpRight();
        else if (PlayerIsRight && PlayerIsBelow)
            JumpDownRight();

    }

    private void JumpDownLeft()
    {
        Vector2 Direction = new Vector2(-0.36f, 0.75f);
        m_Rb.velocity = Direction;
        m_Animator.SetBool("IsGrounded", false);
        m_CurrentSortOrder += 2;
        Debug.Log("JumpDownLeft!");
    }

    private void JumpDownRight()
    {
        Vector2 Direction = new Vector2(0.36f, 0.75f);
        m_Rb.velocity = Direction;
        m_Animator.SetBool("IsGrounded", false);
        m_CurrentSortOrder += 2;
        Debug.Log("JumpDownRight!");
    }

    private void JumpUpRight()
    {
        Vector2 Direction = new Vector2(0.32f, 1.75f);
        m_Rb.velocity = Direction;
        m_Animator.SetBool("IsGrounded", false);
        m_CurrentSortOrder -= 2;
        Debug.Log("JumpUpRight!");
    }
    private void JumpUpLeft()
    {
        Vector2 Direction = new Vector2(-0.32f, 1.75f);
        m_Rb.velocity = Direction;
        m_Animator.SetBool("IsGrounded", false);
        m_CurrentSortOrder -= 2;
        Debug.Log("JumpUpLeft!");
    }
}
