using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance
    public int Score { get; private set; } // The current score (read-only from other scripts)
    public bool IsGameOver { get; private set; } = false; // Tracks whether the game is over

    public GameObject bomb;
    public AudioSource rightSound;
    public AudioSource wrongSound;
    private BombController controllerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controllerScript = bomb.GetComponent<BombController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager instances
        }
    }

    // Method to add score
    public void AddScore(int amount)
    {
        Score += amount;
        Debug.Log($"Score: {Score}"); // Log the score (optional for debugging)
        controllerScript.blinkGreen();
        rightSound.Play();
    }

    public void onWrongAnswer() 
    {
        wrongSound.Play();
    }
    // Method to reset score
    public void ResetScore()
    {
        Score = 0;
    }
    public void GameOver()
    {
        IsGameOver = true;
        Debug.Log("Game Over!");
        UIManager.Instance.ShowGameOverText(); // Call UIManager to surface the "Game Over" text
    }
}
