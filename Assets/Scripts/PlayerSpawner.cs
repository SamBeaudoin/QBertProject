using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform m_SpawnPosition;

    [SerializeField]
    public GameObject m_Player;

    #region Singleton

    public static PlayerSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    private void Start()
    {
        m_SpawnPosition = this.transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnNewObject()
    {
        Instantiate(m_Player, m_SpawnPosition);
    }
    public void NewSpawnRequest()
    {
        Invoke("SpawnNewObject", 1.5f); // Delay spawning by m_SpawnInterval
    }
    public void SetSpawnPosition(Vector3 location)
    {
        m_SpawnPosition.position = location;
    }
}
