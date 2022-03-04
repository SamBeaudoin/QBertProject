using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private int m_CurrentSortOrder;
    private Rigidbody2D m_Rb;
    private Animator m_Animator;
    private Transform m_Transform;

    private float m_JumpInterval = 1.5f;

    public Vector3 m_Offest = new Vector3(0.0f, 0.1f, 0.0f);

    [SerializeField]
    public bool m_JumpingRight;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Transform = this.transform;
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
            m_Transform.position = collision.transform.position + m_Offest;
            //m_Rb.velocity = Vector2.zero;
            JumpDecision();
        }
        if(collision.collider.tag == "Floor")
        {
            Destroy(this.gameObject);
            return;
        }
        m_Animator.SetBool("IsGrounded", true);
        
    }

    private void JumpDecision()
    {
        bool JumpLeft = (Random.Range(0.0f, 1.0f) >= 0.5f);
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
        Vector2 Direction = new Vector2(-0.16f, 0.42f);
        m_Rb.AddForce(Direction, ForceMode2D.Impulse);
        m_Animator.SetBool("IsGrounded", false);
        m_CurrentSortOrder += 2;
        m_JumpingRight = false;
    }

    private void JumpDownRight()
    {
        Vector2 Direction = new Vector2(0.16f, 0.42f);
        // todo: Fix glitch where ball jumps in place and not down pyramid
        m_Rb.AddForce(Direction, ForceMode2D.Impulse);  
        m_Animator.SetBool("IsGrounded", false);
        m_CurrentSortOrder += 2;
        m_JumpingRight = true;
    }
}
