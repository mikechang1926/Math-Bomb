using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } // Singleton instance

    [SerializeField] private TMP_Text tmpText; // Reference to the Game Over text

    [SerializeField] private TMP_Text mainText; // Reference to the Game Over text

    public GameObject playLevelUI;
    public Button playNextButton; 
    private void Awake()
    {
        // Ensure only one instance of UIManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate UIManager instances
        }
    }

    private void Start()
    {
        // Ensure the Game Over text is hidden at the start
        if (tmpText != null)
        {
            tmpText.gameObject.SetActive(false);
        }
    }

    // Show the Game Over text
    public void ShowGameOverText()
    {
        // if (tmpText != null)
        // {
        //     tmpText.text = "Game Over!";
        //     tmpText.gameObject.SetActive(true);
        // }
        ShowPlayLevelUI(GameManager.Instance.currentLevelIndex, true);
    }
    public void ShowPlayLevelUI(int currentLevelIndex, bool includeGameOverText)
    {
        if (includeGameOverText) {
            mainText.text = "<b>Game Over! </b>\n\n";
            mainText.text += "Highest Level " + (currentLevelIndex + 1) + "\n";
            mainText.text += "Your score is " + GameManager.Instance.Score + "";
            playNextButton.GetComponentInChildren<TMP_Text>().text = "Restart Game";
        } else if (currentLevelIndex == 0)
        {
            mainText.text = "Level 1: It's an emergency! Solve as many math problems as quickly as you can to prevent the bomb from blowing up!";
        }
        else
        {
            mainText.text = $"Level {currentLevelIndex + 1}";
        }
        playLevelUI.SetActive(true);
       if (playNextButton != null)
        {
            playNextButton.onClick.AddListener(() => PlayNextLevel(currentLevelIndex, includeGameOverText));
        }
    }
        public void PlayNextLevel(int currentLevelIndex, bool restartGame)
    {
        if (restartGame) {
            RestartGame();
        } else {

        Debug.Log("Loading Next Level...");
        GameManager.Instance.playLevel(currentLevelIndex);
        playLevelUI.SetActive(false);
        
        }
        // Call your level manager to load the next level
        // Example: SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void RestartGame()
{
     Destroy(GameManager.Instance.gameObject); // Remove singleton instance
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

}
