using UnityEngine;
using TMPro;

public class MathProblemManager : MonoBehaviour
{

    [SerializeField] private GameObject problemTextObj; // Reference to the Problem Text
    [SerializeField] private GameObject answerInputObj; // Reference to the Input Field


    [SerializeField] private TMP_Text problemText; // Reference to the Problem Text
    [SerializeField] private TMP_InputField answerInput; // Reference to the Input Field

    private int correctAnswer; // Stores the correct answer for the current problem

    private void Start()
    {
        GenerateNewProblem(); // Generate the first problem
    }
   public void FocusOnInputField()
    {
        if (answerInput != null)
        {
            answerInput.Select(); // Selects the input field
            answerInput.ActivateInputField(); // Ensures the field is active and ready for typing
            Debug.Log("Focused on entry field.");
        }
        else
        {
            Debug.LogError("UIManager: No InputField assigned in the Inspector!");
        }
    }
    private void Update()
    {
        if (GameManager.Instance.isPendingLevelUp || GameManager.Instance.IsGameOver) {
            problemTextObj.SetActive(false);
            answerInputObj.SetActive(false);
            Debug.Log("Level up pending, disabling math problems.");
            return;
        } else 
        {
            problemTextObj.SetActive(true);
            answerInputObj.SetActive(true);
        }
        // Check for user input when Enter/Return is pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckAnswer();
        }
        else {
            FocusOnInputField();
        }
    }

    private void GenerateNewProblem()
    {
        int level = GameManager.Instance.currentLevelIndex;
        Debug.Log($"Generating problem for Level {level}");

        // Scale difficulty faster
        int minRange = 1 + (level * 2);
        int maxRange = 10 + (level * 3);

        int number1 = Random.Range(minRange, maxRange);
        int number2 = Random.Range(minRange, maxRange);

        // Introduce more operators at higher levels
        int operatorType = Random.Range(0, level >= 5 ? 4 : level >= 3 ? 3 : 2);

        if (operatorType == 0) // Addition
        {
            correctAnswer = number1 + number2;
            problemText.text = $"{number1} + {number2} = ?";
        }
        else if (operatorType == 1) // Multiplication
        {
            correctAnswer = number1 * number2;
            problemText.text = $"{number1} × {number2} = ?";
        }
        else if (operatorType == 2) // Subtraction (unlocked at level 3)
        {
            if (number1 < number2) (number1, number2) = (number2, number1);
            correctAnswer = number1 - number2;
            problemText.text = $"{number1} - {number2} = ?";
        }
        else if (operatorType == 3) // Division (unlocked at level 5)
        {
            number1 = Random.Range(minRange, maxRange);
            number2 = Random.Range(1, number1);
            number1 = (number1 / number2) * number2; // Ensure clean division
            correctAnswer = number1 / number2;
            problemText.text = $"{number1} ÷ {number2} = ?";
        }

        // Introduce multi-step problems at level 7+
        if (level >= 7)
        {
            int number3 = Random.Range(minRange, maxRange);
            problemText.text = $"{number1} + {number2} × {number3} = ?";
            correctAnswer = number1 + (number2 * number3);
        }

        answerInput.text = "";
        answerInput.ActivateInputField();
    }

    private void CheckAnswer()
    {
        // Get the user's input and try to parse it to an integer
        if (int.TryParse(answerInput.text, out int userAnswer))
        {
            if (userAnswer == correctAnswer)
            {
                Debug.Log("Correct Answer!");

                // Add a point to the score via GameManager
                GameManager.Instance.AddScore(1);

                // Generate a new math problem
                GenerateNewProblem();
            }
            else
            {
                GameManager.Instance.onWrongAnswer();
                Debug.Log("Incorrect Answer. Try again!");
                answerInput.text = ""; // Clear the input field for retry
                answerInput.ActivateInputField(); // Refocus on the input field
            }
        }
        else
        {
            Debug.Log("Invalid Input. Please enter a number.");
            answerInput.text = ""; // Clear the input field for retry
            answerInput.ActivateInputField(); // Refocus on the input field
        }
    }
}
