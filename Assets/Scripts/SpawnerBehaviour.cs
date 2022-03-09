using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    private Transform m_SpawnPos1;

    private Transform m_SpawnPos2;

    [SerializeField]
    public GameObject m_SpawnObject1;   // Green

    [SerializeField]
    public GameObject m_SpawnObject2;   // Red

    [SerializeField]
    public GameObject m_SpawnObject3;   // Coily

    public float m_SpawnInterval = 15.0f;

    private int m_CoilyCountdown = 3;   

    public bool m_CoilySpawned = false;

    #region Singleton

    public static SpawnerBehaviour Instance;

    private void Awake()
    {
        Instance = this;
        m_SpawnPos1 = this.transform;
        m_SpawnPos2 = this.transform;
    }

    #endregion

    private void Start()
    {
        m_SpawnPos1.position = new Vector3(-0.32f, 1.5f, 0.0f);
        m_SpawnPos2.position = new Vector3(0.16f, 1.5f, 0.0f);
        this.transform.position = Vector3.zero;
        NewSpawnRequest();  // Start Spawn loop by calling first Spawn in Start()
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnNewObject()
    {
        GameObject SpawnObject;

        // Determine if Coily needs to be spawned
        if (!m_CoilySpawned && m_CoilyCountdown % 3 == 0)
        {
            Instantiate(m_SpawnObject3, Random.Range(0.0f, 1.0f) >= 0.5f ? m_SpawnPos1 : m_SpawnPos2);
            m_CoilySpawned = true;
            m_CoilyCountdown = 1;
        }
        // Spawn a Red or Green Ball 50/50 chance
        else
        {
            bool GreenSpawn = (Random.Range(0.0f, 1.0f) >= 0.75f);  // 25% Chance for a Green Ball Spawn
            if (GreenSpawn) SpawnObject = m_SpawnObject1;
            else SpawnObject = m_SpawnObject2;
            Instantiate(SpawnObject, Random.Range(0.0f, 1.0f) >= 0.5f ? m_SpawnPos1 : m_SpawnPos2);
            if (!m_CoilySpawned) m_CoilyCountdown += 1;
        }

        NewSpawnRequest();  // Loop to continue Spawning
    }
    public void NewSpawnRequest()
    {
        Invoke("SpawnNewObject", m_SpawnInterval); // Delay spawning by m_SpawnInterval
    }
}
