using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    public bool m_IsChanged = false;
    private bool m_IsCollisionActive = true;
    public int m_CurrentLayer;

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
        // Activate Collisions

        GetComponent<EdgeCollider2D>().enabled = m_IsCollisionActive;

        // Block Change Animation
        if(m_IsChanged)
        {
            GetComponent<Animator>().SetBool("ChangeBlock", true);
        }
        if(!m_IsChanged)
        {
            GetComponent<Animator>().SetBool("ChangeBlock", false);
        }
    }
}
