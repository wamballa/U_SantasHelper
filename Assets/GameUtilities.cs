using UnityEngine;

public static class GameUtilities
{

    private static readonly string HighScoreKey = "HighScore";

    // Saves the high score
    public static void SaveHighScore(int score)
    {
        int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, score);
            PlayerPrefs.Save();
        }
    }

    // Loads the high score
    public static int LoadHighScore()
    {
        return PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    // Add other utility methods below...
}
