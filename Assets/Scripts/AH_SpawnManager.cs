using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_SpawnManager : MonoBehaviour
{
    //Point of spawn (X)
    private float xLimit = 6f;

    public GameObject[] CarPrefabs;
    public float[] CarYPos;

    //Timers
    private float StartDelay = 0.1f;
    private float RepeatRate = 2f;

    void Start()
    {
        InvokeRepeating("SpawnRandomCar", StartDelay, RepeatRate);
    }

    public Vector3 RandomSpawnPosCar(int RandomSpawnPosX)
    {
        //Variable que guarda de forma Random su spawn en el límite superior o inferior
        float RandomSpawnPosY = Random.Range(0, CarYPos.Length);
        //float RandomSpawnPosX = Random.Range(-xLimit,)
        //Le indicamos que si vale 3 o más aparezca en el límite izquierdo, mientras que si vale menos lo haga en el límite derecho
        if (RandomSpawnPosX >= 3)
        {
            return new Vector3(-xLimit, RandomSpawnPosY, 0);
        }
        else
        {
            return new Vector3(xLimit, RandomSpawnPosY, 0);
        }

    }

    public void SpawnRandomCar()
    {
        int RandomIndex = Random.Range(0, CarPrefabs.Length); //Random car prefab
        int RandomSpawnPosX = Random.Range(0, 2); //Random spawn to the left or right
        GameObject Prefabs = Instantiate(CarPrefabs[RandomIndex], RandomSpawnPosCar(RandomSpawnPosX), CarPrefabs[RandomIndex].transform.rotation); //Random instantiate (carprefab + position)

        if (RandomSpawnPosX == 1)
        {
            Prefabs.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

}
