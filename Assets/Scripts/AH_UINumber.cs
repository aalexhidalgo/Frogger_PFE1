using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AH_UINumber : MonoBehaviour
{
    public bool canvasNumber;
    private float verticalSpeed = 1f;
    private float scaleFactor = 5f;
    public float score;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        if(canvasNumber == true) //Only needed when it's a score number canvas
        {
            scoreText.text = $"+{score}";
        }
    }

    void Update()
    {
        //To make the UI feedback (mysteryBox) movement and scale change
        transform.position = new Vector3(transform.position.x, transform.position.y + verticalSpeed * Time.deltaTime, 0);
        transform.localScale *= 1 - Time.deltaTime / scaleFactor;
    }
}
