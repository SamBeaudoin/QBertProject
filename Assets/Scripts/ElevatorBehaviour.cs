using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorBehaviour : MonoBehaviour
{
    public bool m_PlayerCollision = false;
    private bool m_IsCollisionActive;
    private int m_CurrentLayer;

    [SerializeField]
    private PlayerBehaviour m_Player;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentLayer = GetComponent<SpriteRenderer>().sortingOrder;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_PlayerCollision == false)
        {
            // Activate Collisions
            if (m_Player.m_CurrentSortOrder > m_CurrentLayer)
                m_IsCollisionActive = true;
            else
                m_IsCollisionActive = false;

            GetComponent<EdgeCollider2D>().enabled = m_IsCollisionActive;
        }

        // Lift Animation
        if(m_PlayerCollision)
        {
            GetComponent<Animator>().SetBool("PlayerCollision", true);
            m_Player.m_CurrentSortOrder = 1;
            GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }
}
