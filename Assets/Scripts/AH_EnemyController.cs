using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_EnemyController : MonoBehaviour
{
    public bool noAnim; //For the objects that don't have animator (car, tree, empty parent of turtle)s
    private float spaceLimit = 10f;
    public float speed;

    private Animator enemyAnim;
    private AH_GameManager GameManagerScript;

    void Start()
    {
        enemyAnim = GetComponent<Animator>();
        GameManagerScript = FindObjectOfType<AH_GameManager>();
    }

    void Update()
    {
        //This script affects to all the enemies and interactuable props that have to travel through x axis

        if (transform.position.x > spaceLimit || transform.position.x < -spaceLimit) //Space Limits
        {
            Destroy(gameObject);
        }

        if (GameManagerScript.gameOver == false) //Movement
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else if(GameManagerScript.gameOver == true && noAnim == false) //If we lose the game pauses until we travel to the gameOver scene
        {
            enemyAnim.enabled = false;
        }        
    }
}
