using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_EnemyController : MonoBehaviour
{
    private float spaceLimit = 4.5f;
    public float speed;

    void Start()
    {

    }

    void Update()
    {
        //This affects to all the enemies and interactuable props that have to travel in x axis
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if(transform.position.x > spaceLimit || transform.position.x < -spaceLimit)
        {
            Destroy(gameObject);
        }
    }
    //A medida que pase el tiempo incrementamos la velocidad de los coches
}