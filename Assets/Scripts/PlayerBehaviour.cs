using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private Animator m_Animator;
    private Transform m_Transform;
    private Rigidbody2D m_Rb;
    private GameObject m_CurrentBlock;

    [SerializeField]
    public Vector3 m_Offest = new Vector3(0.0f, 0.1f, 0.0f);


    [SerializeField]
    public List<GameObject> m_LevelBlocks;

    //[SerializeField]
    //[Range(0.0f, 5.0f)]
    //private float m_XStrength = -0.2f;

    //[SerializeField]
    //[Range(0.0f, 5.0f)]
    //private float m_YStrength = 1.75f;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Transform = this.transform;
        m_Rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Keypad1))
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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_Transform.position = new Vector2(0.0f, 0.0f);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_Transform.position = collision.transform.position + m_Offest;
        collision.gameObject.GetComponent<BlockBehaviour>().m_IsChanged = true;
    }

    public void JumpUpLeft()
    {
        Vector2 Direction = new Vector2(-0.32f, 1.75f);
        m_Rb.AddForce(Direction, ForceMode2D.Impulse);
    }

    public void JumpUpRight()
    {
        Vector2 Direction = new Vector2(0.32f, 1.75f);
        m_Rb.AddForce(Direction, ForceMode2D.Impulse);
    }

    public void JumpDownLeft()
    {
        Vector2 Direction = new Vector2(-0.34f, 0.75f);
        m_Rb.AddForce(Direction, ForceMode2D.Impulse);
    }

    public void JumpDownRight()
    {
        Vector2 Direction = new Vector2(0.34f, 0.75f);
        m_Rb.AddForce(Direction, ForceMode2D.Impulse);
    }
}
