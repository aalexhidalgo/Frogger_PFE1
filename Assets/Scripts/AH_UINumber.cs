using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AH_UINumber : MonoBehaviour
{
    public float verticalSpeed = 1f;
    public float scaleFactor = 5f;
    public float score;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText.text = $"+{score}";
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + verticalSpeed * Time.deltaTime, 0);
        transform.localScale *= 1 - Time.deltaTime / scaleFactor;
    }
}
