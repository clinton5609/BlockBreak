using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject gameOverUI;
    public GameObject mainMenuUI;
    private bool isGameOver;
    public static bool isRetrying = false; // for tracking if player is coming from game over 
    [SerializeField] private GameObject playerPrefab; 
    [SerializeField] private Transform playerSpawn; // spawn point of player
    private PlayerHealth player;

    void Start()
    {
        // check if the player is coming from game over screen
        if (isRetrying)
        {
            // skip showing main menu 
            isRetrying = false;
            StartGame();
        }
        else
        {
            // when game launches, show main menu
            MainMenu();
        }
        
    }
    // for start button on main menu 
    public void StartGame()
    {
        // check to see if player is coming from death screen
        isGameOver = false;

        // hide ui
        SetActiveSafe(mainMenuUI, false);
        SetActiveSafe(gameOverUI, false);

        // unpause the game 
        Time.timeScale = 1f;

        // check if the player reference or player object is null 
        if (player == null || player.Equals(null))
        {
            // find player spawn pos 
            var pos = playerSpawn ? playerSpawn.position : Vector3.zero;
            // create player game object
            var go = Instantiate(playerPrefab, pos, Quaternion.identity);
            // create a reference for playerhealth
            player = go.GetComponent<PlayerHealth>();

            // pass this game manager reference into player's script
            if (player)
            {
                player.gameManager = this;
            }

            // assign player as the target for camera follow
            var camFollow = Camera.main ? Camera.main.GetComponent<CameraFollow>() : null;
            if (camFollow) camFollow.SetTarget(go.transform);
        }

    }
    // player health reached 0
    public void GameOver()
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;
        // pause the game
        Time.timeScale = 0f;
        FindObjectOfType<PlayerHealthUI>().HideHealthText();
        // hide main menu and show game over screen
        SetActiveSafe(mainMenuUI, false);
        SetActiveSafe(gameOverUI, true);
    }

    // for retry button 
    // reload the scene
    public void Restart()
    {
        isRetrying = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        isGameOver = false;
        // pause the game
        Time.timeScale = 0f;
        // toggle appropriate uis
        SetActiveSafe(gameOverUI, false);
        SetActiveSafe(mainMenuUI, true);

    }

    public void Quit()
    {
        Application.Quit();
    }
    // for enabling / disabling ui
    private static void SetActiveSafe(GameObject gameObject, bool on)
    {
        if(gameObject != null)
        {
            gameObject.SetActive(on);
        }
    }
}
