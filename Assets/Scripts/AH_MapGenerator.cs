using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_MapGenerator : MonoBehaviour
{
    public List<AH_MapBlock> MapBlocks = new List<AH_MapBlock>(); //List with all the map blocks
    public Transform GameStartPoint;
    public List<AH_MapBlock> currentBlocks = new List<AH_MapBlock>(); //Currentblocks in the scene (clone)

    private void Start()
    {
        GenerateMapBlocks();
    }

    public void AddMapBlock()
    {
        //Random map block instance
        int randomIndex = Random.Range(0, MapBlocks.Count);
        AH_MapBlock currentBlock = (AH_MapBlock)Instantiate(MapBlocks[randomIndex]);

        //currentBlock.transform.SetParent(this.transform, false);
        Vector3 spawnPosition = Vector3.zero;

        if (currentBlocks.Count == 0) //First point of spawn of the map blocks
        {
            spawnPosition = GameStartPoint.position;
        }
        else  //Every map block will appear attached to the end point of the previous map block
        {
            spawnPosition = currentBlocks[currentBlocks.Count - 1].endPoint.position;
        }

        currentBlock.transform.position = spawnPosition;
        currentBlocks.Add(currentBlock);
    }

    public void GenerateMapBlocks() //Generates map blocks until it reaches 7
    {
        for (int i = 0; i < 7; i++) //cambiar n�mero
        {
            AddMapBlock();
        }
    }
}
