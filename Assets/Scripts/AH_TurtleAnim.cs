using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_TurtleAnim : MonoBehaviour
{
    public bool underWater; //Animated bool

    private Animator turtleAnim;
    private AH_GameManager GameManagerScript;

    void Start()
    {
        turtleAnim = GetComponent<Animator>();
        GameManagerScript = FindObjectOfType<AH_GameManager>();
    }

    void Update()
    {
        if (GameManagerScript.gameOver == true) //If we lose the game pauses until we travel to the gameOver scene
        {
            turtleAnim.enabled = false;
        }
    }
}
