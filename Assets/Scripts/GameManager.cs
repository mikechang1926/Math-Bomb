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
    public bool isPendingLevelUp;

    public LevelConfig levelConfig;
    public int currentLevelIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controllerScript = bomb.GetComponent<BombController>();
        isPendingLevelUp = true;
        UIManager.Instance.ShowPlayLevelUI(currentLevelIndex, false);
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
    public void playLevel(int level) 
    {
        currentLevelIndex = level;
        Debug.Log($"Starting Level {level}");
        Debug.Log($"Time Limit: {levelConfig.levels[currentLevelIndex].timeLimit} seconds");
        isPendingLevelUp = false;
        controllerScript.startTicking();
    }

    // Method to add score
    public void AddScore(int amount)
    {
        Score += amount;
        Debug.Log($"Score: {Score}"); // Log the score (optional for debugging)
        controllerScript.blinkGreen();
        rightSound.Play();

        CheckLevelUp(Score);
    }
    public void CheckLevelUp(int currentScore)
    {
        if (currentLevelIndex < levelConfig.levels.Length && 
            currentScore >= levelConfig.levels[currentLevelIndex].scoreLimit)
        {
            LevelUp();
        }
    }
    public void LevelUp()
    {
        currentLevelIndex++;
        if (currentLevelIndex < levelConfig.levels.Length)
        {
            Debug.Log($"Leveled up! Now at Level {levelConfig.levels[currentLevelIndex].level}");
            Debug.Log($"New Time Limit: {levelConfig.levels[currentLevelIndex].timeLimit} seconds");
            isPendingLevelUp = true; // Pause the bomb timer
            UIManager.Instance.ShowPlayLevelUI(currentLevelIndex, false);
        }
        else
        {
            Debug.Log("Max Level Reached!");
            isPendingLevelUp = true; // Pause the bomb timer
            UIManager.Instance.ShowPlayLevelUI(currentLevelIndex, true);
        }
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
