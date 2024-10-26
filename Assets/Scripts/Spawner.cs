using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<Enemy> unitList = new List<Enemy>();
    [SerializeField] List<GameObject> spawnPositions = new List<GameObject>();
    [SerializeField] int unitLimit = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
