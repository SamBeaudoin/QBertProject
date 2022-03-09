using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoilyBehaviour : MonoBehaviour
{

    private int m_CurrentSortOrder;
    private Rigidbody2D m_Rb;
    private Animator m_Animator;
    private Transform m_Transform;

    [SerializeField]
    public GameObject m_CoilySnake;

    private float m_JumpInterval = 0.5f;

    public bool m_IsEgg = true;

    public Vector3 m_Offest = new Vector3(0.0f, 0.1f, 0.0f);

    [SerializeField]
    public bool m_JumpingRight; // To see which direction is Jumping

    [SerializeField]
    public GameObject m_Player;

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

        //if(!m_IsGrounded)
        //{
        //    Debug.Log(this.name + " Velocity In Air: " + m_Rb.velocity.x);
        //}
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

            if(m_CurrentSortOrder == 13)
            {
                Vector2 Direction = new Vector2(0.0f, 1.75f);
                m_Rb.AddForce(Direction, ForceMode2D.Impulse);
                m_Animator.SetBool("IsGrounded", false);
                Invoke("SpawnCoily", 0.4f);
                Destroy(this.gameObject, 0.5f);
                return;
            }

            JumpDecision();
        }
        if (collision.collider.tag == "Floor")
        {
            SpawnerBehaviour.Instance.m_CoilySpawned = false;
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
        Vector2 Direction = new Vector2(-0.36f, 0.55f);
        m_Rb.AddForce(Direction, ForceMode2D.Impulse);
        m_Animator.SetBool("IsGrounded", false);
        m_CurrentSortOrder += 2;
        m_JumpingRight = false;
    }

    private void JumpDownRight()
    {
        Vector2 Direction = new Vector2(0.36f, 0.55f);
        m_Rb.AddForce(Direction, ForceMode2D.Impulse);
        m_Animator.SetBool("IsGrounded", false);
        m_CurrentSortOrder += 2;
        m_JumpingRight = true;
    }

    private void SpawnCoily()
    {
        Transform SpawnPoint = this.transform;
        Instantiate(m_CoilySnake, SpawnPoint);
    }
}
