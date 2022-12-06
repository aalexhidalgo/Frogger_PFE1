using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_MapGenerator : MonoBehaviour
{
    public List<AH_MapBlock> MapBlocks = new List<AH_MapBlock>(); //List with all the map blocks
    public Transform GameStartPoint;
    public List<AH_MapBlock> currentBlocks = new List<AH_MapBlock>(); //Currentblocks in the scene (clone)
    private GameObject FirstMapBlock;

    private void Start()
    {
        GenerateMapBlocks();
        FirstMapBlock = GameObject.Find("FirstMapBlock");
    }

    public void AddMapBlock()
    {
        //Random map block instance
        int randomIndex = Random.Range(0, MapBlocks.Count);
        AH_MapBlock currentBlock = (AH_MapBlock)Instantiate(MapBlocks[randomIndex]);
        currentBlock.transform.SetParent(this.transform, false);

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

    public void GenerateMapBlocks() //Generates map blocks until it reaches 5
    {
        for (int i = 0; i < 2; i++)
        {
            AddMapBlock();
        }
    }

    public void RemoveBlock()
    {
        //Removes the map block on the position 0 from the list and also from the scene.
        AH_MapBlock oldestBlock = currentBlocks[0];
        currentBlocks.Remove(oldestBlock);
        Destroy(oldestBlock.gameObject);
        Destroy(FirstMapBlock);
    }
}
