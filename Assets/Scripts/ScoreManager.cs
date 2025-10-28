using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [Header("UI")]
    public TMP_Text coinsText;
    public TMP_Text scoreText;
    public TMP_Text gameOverScoreText;
    public TMP_Text highScoreText;

    // counters
    private int coins = 0;
    private int score = 0;
    private int gameOverScore = 0;
    private int highScore = 0;

    // properties for other scripts to use
    public int Coins => coins;
    public int Score => score;
    public int GameOverScoreText => gameOverScore;
    public int HighScore => highScore;

    [Tooltip("Check this box to reset the High Score in Player Prefs")]
    public bool resetHighScoreNow = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // load previous high score if any, from player prefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        // update ui with initial values
        UpdateUI();
    }

    // used for when player collects coins
    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();
    }


    public bool TrySpend(int amount)
    {
        // check if player has enough coins 
        if (coins < amount)
        {
            return false;
        }

        //if player can buy, subtract proper amount
        coins -= amount;
        // update ui
        UpdateUI();
        return true;
    }
    // for point system after killing an enemy
    public void AddScore(int amount)
    {
        score += amount;
        // want to simultaneously update this
        // mainly used for the game over screen
        gameOverScore = score;
        
        // try to update high score
        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        UpdateUI();
    }
    
    public void ResetScore()
    {
        score = 0;
        gameOverScore = score;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (coinsText != null)
        {
            coinsText.text = "Coins: " + coins.ToString();
        }
        // in game score ui 
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
        // game over screen score
        if (gameOverScoreText != null)
        {
            gameOverScoreText.text = "Score: " + gameOverScore.ToString();
        }
        // game over high score display
        if (highScoreText != null)
        {
            highScoreText.text = "Highest Score: " + highScore.ToString();
        }
    }

    // allows for resetting high score in inspector
    void OnDrawGizmos()
    {
        if (resetHighScoreNow)
        {
            resetHighScoreNow = false;
            PlayerPrefs.SetInt("HighScore", 0);
            highScore = 0;
            UpdateUI();
        }
    }
    
    public void ResetRunTotals(bool resetCoinsToo = true)
    {
        score = 0;
        gameOverScore = 0;
        if (resetCoinsToo) coins = 0;
        UpdateUI();
    }
    

}
