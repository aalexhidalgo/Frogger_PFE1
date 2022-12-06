using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_MysteryBox : MonoBehaviour
{
    private float lifeTime = 10f;
    public GameObject mysteryBoxparticle;

    private Animator mysteryBoxAnim;
    private AH_GameManager GameManagerScript;

    void Start()
    {
        mysteryBoxAnim = GetComponent<Animator>();
        GameManagerScript = FindObjectOfType<AH_GameManager>();
        Destroy(gameObject, lifeTime);
    }
    
    void Update()
    {
        if (GameManagerScript.gameOver)
        {
            mysteryBoxAnim.enabled = false;
        }

        if(lifeTime == 9f)
        {
            Instantiate(mysteryBoxparticle, transform.position, transform.rotation);
        }
    }
}
