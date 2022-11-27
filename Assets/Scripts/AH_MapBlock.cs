using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_MapBlock : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    private AH_GameManager GameManagerScript;
    private AH_MapGenerator MapGeneratorScript;

    void Start()
    {
        GameManagerScript = FindObjectOfType<AH_GameManager>();
        MapGeneratorScript = FindObjectOfType<AH_MapGenerator>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManagerScript.UpdateScore(100); //After we reach a new zone, 100 points will be add as a reward to our current score
            MapGeneratorScript.AddMapBlock();
            MapGeneratorScript.RemoveBlock();
        }
    }
}
