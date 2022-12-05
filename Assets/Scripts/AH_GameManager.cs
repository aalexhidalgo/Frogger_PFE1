using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class AH_GameManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private float TimeCounter;

    public Image lifeImage;
    public Sprite[] lifeSpriteArray;

    public TextMeshProUGUI scoreText;
    public int scoreCounter;

    public bool gameOver;

    public GameObject[] skinArray;
    private CinemachineVirtualCamera cvCamera;

    private AH_PlayerController playerControllerScript;

    void Awake()
    {
        LoadSkin();
    }
    void Start()
    {
        playerControllerScript = FindObjectOfType<AH_PlayerController>();
        cvCamera = FindObjectOfType<CinemachineVirtualCamera>();
        cvCamera.Follow = skinArray[AH_DataPersistance.PlayerStats.skinSelected].transform;
    }

    void Update()
    {
        if (playerControllerScript.active == true) //The time doesn't start counting if we didn't move
        {
            UpdateTime();
        }
    }

    void UpdateTime()
    {
        TimeCounter += Time.deltaTime;
        float minutes = Mathf.FloorToInt(TimeCounter / 60);
        float seconds = Mathf.FloorToInt(TimeCounter % 60);
        float miliseconds = (TimeCounter % 1) * 1000;
        timeText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, miliseconds);

        AH_DataPersistance.PlayerStats.minutes = minutes;
        AH_DataPersistance.PlayerStats.seconds = seconds;
        AH_DataPersistance.PlayerStats.miliseconds = miliseconds;
    }

    public void UpdateScore(int score)
    {
        scoreCounter += score;
        scoreText.text = scoreCounter.ToString();
        AH_DataPersistance.PlayerStats.score = scoreCounter;
    }

    public void GameOver()
    {
        AH_DataPersistance.PlayerStats.SaveForFutureGames(); //Before we die we save the values in data persistance
        SceneManager.LoadScene("Game Over");
    }

    private void LoadSkin()
    {
        skinArray[AH_DataPersistance.PlayerStats.skinSelected].SetActive(true);
    }

}


