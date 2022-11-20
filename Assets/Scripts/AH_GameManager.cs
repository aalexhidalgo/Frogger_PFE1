using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class AH_GameManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private float TimeCounter;

    public Image lifeImage;
    public Sprite[] lifeSpriteArray;

    private AH_PlayerController playerControllerScript;

    void Start()
    {
        playerControllerScript = FindObjectOfType<AH_PlayerController>();

    }

    void Update()
    {
        if (playerControllerScript.active == true) //Te time doesn't start counting if we didn't move
        {
            UpdateTime();
        }
    }

    public void UpdateTime()
    {
        TimeCounter += Time.deltaTime;
        timeText.text = TimeCounter.ToString(); //With Mathf.Round we get the number without the miliseconds        
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
