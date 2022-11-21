using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_DataPersistance : MonoBehaviour
{
    //Shared instance
    public static AH_DataPersistance PlayerStats;

    //Variables
    public static int score;

    public static float minutes;
    public static float seconds;
    public static float miliseconds;

    void Awake()
    {
        // Si la instancia no existe
        if (PlayerStats == null)
        {
            // Configuramos la instancia
            PlayerStats = this;
            // Nos aseguramos de que no sea destruida con el cambio de escena
            DontDestroyOnLoad(PlayerStats);
        }
        else
        {
            // Como ya existe una instancia, destruimos la copia
            Destroy(this);
        }
    }

    public static void SaveForFutureGames()
    {
        PlayerPrefs.SetInt("Score", score);

        PlayerPrefs.SetFloat("Minutes", minutes);
        PlayerPrefs.SetFloat("Seconds", seconds);
        PlayerPrefs.SetFloat("Miliseconds", miliseconds);
    }
}
