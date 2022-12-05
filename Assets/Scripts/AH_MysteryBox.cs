using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_MysteryBox : MonoBehaviour
{
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
    }
}
