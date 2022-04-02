using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private int m_CurrentSortOrder;
    private Rigidbody2D m_Rb;
    private Animator m_Animator;
    private Transform m_Transform;

    private float m_JumpInterval = 0.5f;

    public Vector3 m_Offest = new Vector3(0.0f, 0.1f, 0.0f);

    private bool m_IsGrounded;

    [SerializeField]
    public bool m_JumpingRight;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Transform = this.transform;
        this.transform.SetParent(null);
        m_Rb = GetComponent<Rigidbody2D>();
        m_CurrentSortOrder = 3;
    }

    // Update is called once per frame
    void Update()
    {
        // Sorting Order update
        GetComponent<SpriteRenderer>().sortingOrder = m_CurrentSortOrder;

    }

    // Collisions
    // Block Snapping for Snappy-ness
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "LevelBlocks")
        {
            m_Animator.SetBool("IsGrounded", true);
            m_Transform.position = collision.transform.position + m_Offest;
            m_Rb.velocity = Vector2.zero;
            m_IsGrounded = true;
            JumpDecision();
        }
        if(collision.collider.tag == "Floor")
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void JumpDecision()
    {
        bool JumpLeft = (Random.value >= 0.5f);
        if (JumpLeft)
        {
            Invoke("JumpDownLeft", m_JumpInterval);
        }
        else
        {
            Invoke("JumpDownRight", m_JumpInterval);
        }
        
    }

    private void JumpDownLeft()
    {
        Vector2 Direction = new Vector2(-0.4f, 0.6f);
        //m_Rb.AddForce(Direction, ForceMode2D.Impulse);
        m_Rb.velocity = Direction;  
        m_Animator.SetBool("IsGrounded", false);
        m_CurrentSortOrder += 2;
        m_IsGrounded = false;
        m_JumpingRight = false;
    }

    private void JumpDownRight()
    {
        Vector2 Direction = new Vector2(0.4f, 0.6f);
        m_Rb.velocity = Direction;  
        m_Animator.SetBool("IsGrounded", false);
        m_CurrentSortOrder += 2;
        m_IsGrounded = false;
        m_JumpingRight = true;
    }
}
