using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    public const int HIGHSCORE_COUNT = 5;

    public static List<int> ReadHighscores() // Returns all saved highscores, sorted from highest to lowest
    {
        List<int> loadedScores = new List<int>();
        for(int i = 0; i < HIGHSCORE_COUNT; i++)
        {
            string lookedUpPref = "HS" + i;
            int loadedValue = 0;
            if(PlayerPrefs.HasKey(lookedUpPref))
            {
                loadedValue = PlayerPrefs.GetInt(lookedUpPref);
            }
            loadedScores.Insert(i, loadedValue);
        }

        for(int i = 0; i < loadedScores.Count; i++)
        {
            Debug.Log("Loaded Highscore #" + i + " : " + loadedScores[i]);
        }        

        return loadedScores;
    }

    public static void SaveHighscores(List<int> scores) // Assumes that scores are sorted
    {
        for(int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetInt("HS" + i, scores[i]);
        }

        for (int i = 0; i < scores.Count; i++)
        {
            Debug.Log("Saved Highscore #" + i + " : " + scores[i]);
        }

        PlayerPrefs.Save();
    }

}
