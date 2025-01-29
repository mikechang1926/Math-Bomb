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

    private void Update()
    {
        if (GameManager.Instance.IsGameOver)
        {
            problemTextObj.SetActive(false);
            answerInputObj.SetActive(false);
        }
        // Check for user input when Enter/Return is pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckAnswer();
        }
    }

    private void GenerateNewProblem()
    {
        // Generate two random numbers for the math problem
        int number1 = Random.Range(1, 10); // Random number between 1 and 9
        int number2 = Random.Range(1, 10); // Random number between 1 and 9

        // Randomly pick an operator (addition or multiplication)
        int operatorType = Random.Range(0, 2); // 0 for addition, 1 for multiplication

        if (operatorType == 0) // Addition
        {
            correctAnswer = number1 + number2;
            problemText.text = $"{number1} + {number2} = ?";
        }
        else // Multiplication
        {
            correctAnswer = number1 * number2;
            problemText.text = $"{number1} x {number2} = ?";
        }

        // Clear the input field for the new problem
        answerInput.text = "";
        answerInput.ActivateInputField(); // Focus on the input field
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
