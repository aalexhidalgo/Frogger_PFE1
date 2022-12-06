using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_EnemyController : MonoBehaviour
{
    private float spaceLimit = 10f;
    public float speed;
    private Vector2 direction = Vector2.right;

    private Animator enemyAnim;
    private AH_GameManager GameManagerScript;

    void Start()
    {
        enemyAnim = GetComponent<Animator>();
        GameManagerScript = FindObjectOfType<AH_GameManager>();
    }

    void Update()
    {
        //This affects to all the enemies and interactuable props that have to travel through x axis

        if(transform.position.x > spaceLimit || transform.position.x < -spaceLimit)
        {
            Destroy(gameObject);
        }

        if (GameManagerScript.gameOver == false)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
        /*else
        {
            enemyAnim.enabled = false;
        }*/        
    }
}
