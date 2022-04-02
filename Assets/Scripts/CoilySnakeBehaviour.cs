using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoilySnakeBehaviour : MonoBehaviour
{
    private int m_CurrentSortOrder;
    private Rigidbody2D m_Rb;
    private Animator m_Animator;
    private Transform m_Transform;
    bool m_DecisionMade = false;

    private float m_JumpInterval = 0.7f;

    private float m_FailsafeDecision = 7.5f;

    public Vector3 m_Offset = new Vector3(0.0f, 0.25f, 0.0f);   

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

        // Kill Floor Check
        KillFloorCheck();

        // Failsafe Decision making
        if(!m_DecisionMade)
        {
            m_FailsafeDecision -= Time.deltaTime;

            if (m_FailsafeDecision < 0f)
            {
                JumpDecision();
                m_FailsafeDecision = 7.5f;
            }
        }
    }

    // Collisions
    // Block Snapping for Snappy-ness
    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_Transform.position = collision.transform.position + m_Offset;
        if (collision.collider.tag == "LevelBlocks")
        {
            if (collision.collider.GetComponent<BlockBehaviour>().m_CurrentLayer > m_CurrentSortOrder)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                return;
            }
            m_Animator.SetBool("IsGrounded", true);
            m_Transform.position = collision.transform.position + m_Offset;
            m_CurrentBlock = collision.gameObject;
            m_CurrentSortOrder = m_CurrentBlock.GetComponent<BlockBehaviour>().m_CurrentLayer + 1;
            m_Rb.velocity = Vector2.zero;

            if(!m_DecisionMade)
                JumpDecision();
        }
        //if (collision.collider.tag == "RedBall" || collision.collider.tag == "GreenBall")
        //{
        //    Destroy(collision.collider.gameObject);
        //}
    }


    private void JumpDecision()
    {
        

        bool JumpRandom = (Random.Range(0.0f, 1.0f) >= 0.5f);
        bool PlayerIsLeft = false;
        bool PlayerIsRight = false;
        bool PlayerIsAbove = false;
        bool PlayerIsBelow = false;
        m_DecisionMade = true;

        GameObject PlayerBlock = m_Player.GetComponent<PlayerBehaviour>().m_CurrentBlock;
        float BlockX = PlayerBlock.transform.position.x;
        float BlockY = PlayerBlock.transform.position.y;

        if (PlayerBlock == null)
        {
            m_DecisionMade = false;
            return;
        }

        if (PlayerBlock.name == "Elevator_Left")
        {
            BlockX = -0.8f;
            BlockY = 0.064f;
        }
        else if (PlayerBlock.name == "Elevator_Right")
        {
            BlockX = 0.8f;
            BlockY = 0.064f;
        }


        if (m_CurrentBlock.transform.position.x > BlockX)  // Player is to the Left
        {
            PlayerIsLeft = true;
        }
        if (m_CurrentBlock.transform.position.x < BlockX)  // Player is to the Right
        {
            PlayerIsRight = true;
        }
        if (m_CurrentBlock.transform.position.y > BlockY)  // Player is Below
        {
            PlayerIsBelow = true;
        }
        if (m_CurrentBlock.transform.position.y < BlockY)  // Player is Above
        {
            PlayerIsAbove = true;
        }

        if (PlayerIsAbove && !(PlayerIsLeft || PlayerIsRight))   // Player is Straight Above
        {
            if (JumpRandom)
                Invoke("JumpUpLeft", m_JumpInterval);
            else
                Invoke("JumpUpRight", m_JumpInterval);
            return;
        }

        if (PlayerIsBelow && !(PlayerIsLeft || PlayerIsRight))   // Player is Straight Below
        {
            if (JumpRandom)
                Invoke("JumpDownLeft", m_JumpInterval);
            else
                Invoke("JumpDownRight", m_JumpInterval);
            return;
        }

        if (PlayerIsRight && !(PlayerIsAbove || PlayerIsBelow))   // Player is Straight Right
        {
            if (JumpRandom)
                Invoke("JumpDownRight", m_JumpInterval);
            else
                Invoke("JumpUpRight", m_JumpInterval);
            return;
        }

        if (PlayerIsLeft && !(PlayerIsAbove || PlayerIsBelow))   // Player is Straight Left
        {
            if (JumpRandom)
                Invoke("JumpDownLeft", m_JumpInterval);
            else
                Invoke("JumpUpLeft", m_JumpInterval);
            return;
        }

        if (PlayerIsLeft && PlayerIsAbove)
            Invoke("JumpUpLeft", m_JumpInterval);
        else if (PlayerIsLeft && PlayerIsBelow)
            Invoke("JumpDownLeft", m_JumpInterval);
        else if (PlayerIsRight && PlayerIsAbove)
            Invoke("JumpUpRight", m_JumpInterval);
        else if (PlayerIsRight && PlayerIsBelow)
            Invoke("JumpDownRight", m_JumpInterval);

    }

    private void KillFloorCheck()
    {
        if (m_Rb.position.y <= -0.75f)
        {
            m_Player.GetComponent<PlayerBehaviour>().m_Score += 500;
            Debug.Log("Coily Is Dead!");
            SpawnerBehaviour.Instance.m_CoilySpawned = false;
            SpawnerBehaviour.Instance.KillAllChildren();
            SpawnerBehaviour.Instance.CancelInvoke();
            SpawnerBehaviour.Instance.NewSpawnRequest();
            // Coily Falls
        }
    }

    private void JumpDownLeft()
    {
        Vector2 Direction = new Vector2(-0.36f, 0.75f);
        m_Rb.velocity = Direction;
        m_Animator.SetBool("IsGrounded", false);
        m_CurrentSortOrder += 2;
        m_DecisionMade = false;
    }

    private void JumpDownRight()
    {
        Vector2 Direction = new Vector2(0.36f, 0.75f);
        m_Rb.velocity = Direction;
        m_Animator.SetBool("IsGrounded", false);
        m_CurrentSortOrder += 2;
        m_DecisionMade = false;
    }

    private void JumpUpRight()
    {
        Vector2 Direction = new Vector2(0.32f, 1.8f);
        m_Rb.velocity = Direction;
        m_Animator.SetBool("IsGrounded", false);
        m_CurrentSortOrder -= 2;
        m_DecisionMade = false;
    }
    private void JumpUpLeft()
    {
        Vector2 Direction = new Vector2(-0.32f, 1.8f);
        m_Rb.velocity = Direction;
        m_Animator.SetBool("IsGrounded", false);
        m_CurrentSortOrder -= 2;
        m_DecisionMade = false;
    }
}
