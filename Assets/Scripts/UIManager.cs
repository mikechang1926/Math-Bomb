using UnityEngine;
using TMPro; // Import TextMeshPro for UI

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } // Singleton instance

    [SerializeField] private TMP_Text gameOverText; // Reference to the Game Over text

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
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
    }

    // Show the Game Over text
    public void ShowGameOverText()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
        }
    }
}
