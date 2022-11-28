using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_SpawnManager1 : MonoBehaviour
{
    //Script bueno
    public bool road, water;
    public bool left, right;
    public GameObject[] CarPrefabs, WaterPrefabs;

    //Timers
    private float StartDelay = 0.1f;
    private float RepeatRate = 2f;


    void Start ()
    {
        InvokeRepeating("SpawnRandomPrefab", StartDelay, RepeatRate);
    }

    public void SpawnRandomPrefab()
    {
        if (road == true) //Car prefabs
        {
            int RandomIndex = Random.Range(0, CarPrefabs.Length);
            GameObject Prefabs = Instantiate(CarPrefabs[RandomIndex], transform.position, CarPrefabs[RandomIndex].transform.rotation);

            if (right == true)
            {
                Prefabs.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }
        else // Water prefabs
        {
            int RandomIndex = Random.Range(0, WaterPrefabs.Length);
            GameObject Prefabs = Instantiate(WaterPrefabs[RandomIndex], transform.position, WaterPrefabs[RandomIndex].transform.rotation);

            if (right == true)
            {
                Prefabs.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }


    }
}
