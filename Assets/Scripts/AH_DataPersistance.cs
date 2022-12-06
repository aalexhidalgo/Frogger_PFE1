using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_DataPersistance : MonoBehaviour
{
    //Shared instance
    public static AH_DataPersistance PlayerStats;

    //Variables
    public int score;
    public int highscore = 0;

    public float minutes;
    public float seconds;
    public float miliseconds;

    public int skinSelected = 0;

    void Awake()
    {
        //If the instance doesn't exist
        if (PlayerStats == null)
        {
            //We set the instance
            PlayerStats = this;
            //We make sure to not destroy it after the scene change
            DontDestroyOnLoad(PlayerStats);
        }
        else
        {
            //Because a instance already exists, we deastroy the cloned one
            Destroy(this);
        }
    }

    public void SaveForFutureGames()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("High_Score", highscore);

        PlayerPrefs.SetFloat("Minutes", minutes);
        PlayerPrefs.SetFloat("Seconds", seconds);
        PlayerPrefs.SetFloat("Miliseconds", miliseconds);

        PlayerPrefs.SetInt("Skin_Selected", skinSelected);
    }
}
