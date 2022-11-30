using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_MapBlock : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    private AH_MapGenerator MapGeneratorScript;

    void Start()
    {
        MapGeneratorScript = FindObjectOfType<AH_MapGenerator>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            MapGeneratorScript.AddMapBlock();
            MapGeneratorScript.RemoveBlock();
        }
    }
}
