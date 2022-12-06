using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_SpawnManager : MonoBehaviour
{
    public bool road, water, ground, mysteryBox;
    public bool left, right;
    public GameObject[] RoadPrefabs, WaterPrefabs; //Road (5 cars), Water (tree blocks, crocodile, turtles, seal)
    public GameObject GroundPrefab, MysteryBox; //Snake and MysteryBox
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
            if (road == true) 
            {
                //Car prefabs
                int RandomIndex = Random.Range(0, RoadPrefabs.Length);
                GameObject Prefabs = Instantiate(RoadPrefabs[RandomIndex], transform.position, RoadPrefabs[RandomIndex].transform.rotation);
                Prefabs.transform.SetParent(this.transform, true);
                RightDirection(Prefabs);
            }
            else if (water == true)// Water prefabs
            {
                int RandomIndex = Random.Range(0, WaterPrefabs.Length);
                GameObject Prefabs = Instantiate(WaterPrefabs[RandomIndex], transform.position, WaterPrefabs[RandomIndex].transform.rotation);
                Prefabs.transform.SetParent(this.transform, true);
                RightDirection(Prefabs);
            }
            else if (ground == true)
            {
                GameObject Prefabs = Instantiate(GroundPrefab, transform.position, GroundPrefab.transform.rotation);
                Prefabs.transform.SetParent(this.transform, true);
                RightDirection(Prefabs);
            }
            else if (mysteryBox == true)
            {
                //Mystery Box
                int randomIndx = Random.Range(-4, 4);
                Vector3 randomPosX = new Vector3(randomIndx, transform.position.y);
                GameObject PrefabBox = Instantiate(MysteryBox, randomPosX, transform.rotation);
                PrefabBox.transform.SetParent(this.transform, true);
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
