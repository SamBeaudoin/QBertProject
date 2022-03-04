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

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Transform = this.transform;
        this.transform.SetParent(null);
        m_Rb = GetComponent<Rigidbody2D>();
        m_CurrentSortOrder = 13;
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

        m_Transform.position = collision.transform.position + m_Offset;


        //if (collision.collider.tag == "LevelBlocks")
        //{
        //    m_Animator.SetBool("IsGrounded", true);
        //    m_Transform.position = collision.transform.position + m_Offset;
        //    m_Rb.velocity = Vector2.zero;

        //    JumpDecision();
        //}
        //if (collision.collider.tag == "Floor")
        //{
        //    SpawnerBehaviour.Instance.m_CoilySpawned = false;
        //    Destroy(this.gameObject);
        //    return;
        //}

    }

    private void JumpDecision()
    {
        //bool JumpLeft = (Random.Range(0.0f, 1.0f) >= 0.5f);
        //if (JumpLeft)
        //{
        //    Invoke("JumpDownLeft", m_JumpInterval);
        //}
        //else
        //{
        //    Invoke("JumpDownRight", m_JumpInterval);
        //}
    }

    private void JumpDownLeft()
    {
        Vector2 Direction = new Vector2(-0.36f, 0.55f);
        m_Rb.AddForce(Direction, ForceMode2D.Impulse);
        m_Animator.SetBool("IsGrounded", false);
        m_CurrentSortOrder += 2;
    }

    private void JumpDownRight()
    {
        Vector2 Direction = new Vector2(0.36f, 0.55f);
        m_Rb.AddForce(Direction, ForceMode2D.Impulse);
        m_Animator.SetBool("IsGrounded", false);
        m_CurrentSortOrder += 2;
    }

}
