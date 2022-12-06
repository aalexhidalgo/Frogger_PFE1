using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_MysteryBox : MonoBehaviour
{
    private float lifeTime = 10f;
    public GameObject mysteryBoxparticle;
    private bool start = true;

    private Animator mysteryBoxAnim;
    private AH_GameManager GameManagerScript;

    void Start()
    {
        mysteryBoxAnim = GetComponent<Animator>();
        GameManagerScript = FindObjectOfType<AH_GameManager>();
    }
    
    void Update()
    {
        if (GameManagerScript.gameOver)
        {
            mysteryBoxAnim.enabled = false;
        }

        if(start == true)
        {
            StartCoroutine("Destroy");
        }
    }

    private IEnumerator Destroy()
    {
        start = false;
        yield return new WaitForSeconds(lifeTime);
        Instantiate(mysteryBoxparticle, transform.position, transform.rotation);       
        Destroy(gameObject);
    }


}
