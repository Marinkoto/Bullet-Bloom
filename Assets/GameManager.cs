using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int score;
    public int highScore;

    public static Action<int> OnScoreChange;
    public static Action<int> OnHighScoreChange;

    private const string HighScoreKey = "HighScore";

    private void Start()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        OnHighScoreChange?.Invoke(highScore);
    }

    public void AddScore(int amount = 1)
    {
        score += amount;
        OnScoreChange?.Invoke(score);
    }

    public void CheckForHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
            OnHighScoreChange?.Invoke(highScore);
        }
    }
    public void EndGame()
    {
        CheckForHighScore();
    }
}