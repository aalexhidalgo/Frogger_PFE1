using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class AH_MenuManager : MonoBehaviour
{
    public TextMeshProUGUI scoreTimeText;

    public void StartButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitButton()
    {
        //Maria
    }

    void Start()
    {
        Load_Time_Score();
    }

    private void Load_Time_Score() //Por hacer
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            scoreTimeText.text = (PlayerPrefs.GetFloat("Score")).ToString();
        }
    }
}
