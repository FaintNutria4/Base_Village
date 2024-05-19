using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{

    public enum SpawnMode
    {
        Timer,
        Trigger
    }

    public SpawnMode spawnMode;
    public GameObject prefab;

    public float timeUntilSpawn;
    private float timer;
    public float unitsPerSpawn = 1;

    public void Spawn()
    {
        for (int i = 0; i < unitsPerSpawn; i++) Instantiate(prefab, transform.position, transform.rotation);
    }
    // Start is called before the first frame update
    void Start()
    {
        timer = Time.time + timeUntilSpawn;
    }

    // Update is called once per frame
    void Update()
    {

        if (SpawnMode.Timer == spawnMode && timer <= Time.time)
        {
            Spawn();
            timer = Time.time + timeUntilSpawn;
        }
    }
}
