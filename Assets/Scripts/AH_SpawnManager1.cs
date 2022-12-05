using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_SpawnManager1 : MonoBehaviour
{
    //Script bueno
    public bool road, water, ground;
    public bool left, right;
    public GameObject[] RoadPrefabs, WaterPrefabs; //Road (5 cars), Water (tree blocks, crocodile, turtles, seal)
    public GameObject GroundPrefab; //Snake
    public GameObject MysteryBox; //Más tarde

    //Timers
    private float StartDelay = 0.1f;
    public float RepeatRate = 2f;

    private AH_GameManager GameManagerScript;

    void Start ()
    {
        GameManagerScript = FindObjectOfType<AH_GameManager>();
        InvokeRepeating("SpawnRandomPrefab", StartDelay, RepeatRate);
    }

    public void SpawnRandomPrefab()
    {
        if(GameManagerScript.gameOver == false)
        {
            if (road == true) //Car prefabs
            {
                int RandomIndex = Random.Range(0, RoadPrefabs.Length);
                GameObject Prefabs = Instantiate(RoadPrefabs[RandomIndex], transform.position, RoadPrefabs[RandomIndex].transform.rotation);

                if (right == true)
                {
                    Prefabs.transform.rotation = Quaternion.Euler(0, 0, 180);
                }
            }
            else if (water == true)// Water prefabs
            {
                int RandomIndex = Random.Range(0, WaterPrefabs.Length);
                GameObject Prefabs = Instantiate(WaterPrefabs[RandomIndex], transform.position, WaterPrefabs[RandomIndex].transform.rotation);

                if (right == true)
                {
                    Prefabs.transform.rotation = Quaternion.Euler(0, 0, 180);
                }
            }
            else if (ground == true)
            {
                GameObject Prefabs = Instantiate(GroundPrefab, transform.position, GroundPrefab.transform.rotation);
                RightDirection(Prefabs);
            }
        }     
    }

    public void RightDirection(GameObject Prefabs)
    {
        if (right == true)
        {
            Prefabs.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }
}
