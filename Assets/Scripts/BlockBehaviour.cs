using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    public bool m_IsChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
