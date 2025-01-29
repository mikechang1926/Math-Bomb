using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class ScoreController : MonoBehaviour
{
    private TMP_Text scoreText; // Reference to the TextMeshPro component

    private void Start()
    {        
        // Automatically find the TextMeshPro component on the same GameObject
        scoreText = GetComponent<TMP_Text>();

        // Update the score UI at the start
        UpdateScoreText();
    }

    private void Update()
    {
        // Continuously update the score text (optional, or use an event-based approach)
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {GameManager.Instance.Score}";
        }
    }
}
