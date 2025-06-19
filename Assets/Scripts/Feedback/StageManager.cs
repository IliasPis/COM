using UnityEngine;
using TMPro;

public class StageManager : MonoBehaviour
{
    [Header("Stages")]
    public GameObject homeStage; // DragDropQuiz GameObject for Home Stage
    public GameObject schoolStage; // DragDropQuiz_Locations GameObject for School Stage
    public GameObject parkStage; // DragDropQuiz GameObject for Park Stage

    [Header("Feedback Text Objects")]
    public TextMeshProUGUI homeFeedbackText; // Text object for Home Stage feedback
    public TextMeshProUGUI schoolFeedbackText; // Text object for School Stage feedback
    public TextMeshProUGUI parkFeedbackText; // Text object for Park Stage feedback

    [Header("High and Low Feedback")]
    public string homeHighFeedback; // High feedback text for Home Stage
    public string homeLowFeedback; // Low feedback text for Home Stage
    public string schoolHighFeedback; // High feedback text for School Stage
    public string schoolLowFeedback; // Low feedback text for School Stage
    public string parkHighFeedback; // High feedback text for Park Stage
    public string parkLowFeedback; // Low feedback text for Park Stage

    [Header("Player Name Settings")]
    public GameObject boyPlayerPrefsObject; // The GameObject with the SavePrefs script for the boy
    public GameObject girlPlayerPrefsObject; // The GameObject with the SavePrefs script for the girl
    public TextMeshProUGUI playerNameText; // The Text object where the name will be displayed

    private DragDropQuiz homeStageScript; // Script reference for Home Stage
    private DragDropQuiz_Locations schoolStageScript; // Script reference for School Stage
    private DragDropQuiz parkStageScript; // Script reference for Park Stage

    void Start()
    {
        // Get references to the stage scripts
        homeStageScript = homeStage.GetComponent<DragDropQuiz>();
        schoolStageScript = schoolStage.GetComponent<DragDropQuiz_Locations>();
        parkStageScript = parkStage.GetComponent<DragDropQuiz>();

        if (homeStageScript == null || schoolStageScript == null || parkStageScript == null)
        {
            Debug.LogError("One or more stage scripts are not correctly assigned.");
            return;
        }

        Debug.Log("StageManager initialized correctly.");

        // Fetch and display the player's name
        DisplayPlayerName();
    }

    public void EvaluateStages()
    {
        Debug.Log("EvaluateStages called.");

        // Evaluate Home Stage
        EvaluateStage(
            homeStageScript.score, // Use the public score variable
            homeFeedbackText,
            homeHighFeedback,
            homeLowFeedback,
            12,
            "Home"
        );

        // Evaluate School Stage
        EvaluateStage(
            schoolStageScript.score, // Use the public score variable
            schoolFeedbackText,
            schoolHighFeedback,
            schoolLowFeedback,
            12,
            "School"
        );

        // Evaluate Park Stage
        EvaluateStage(
            parkStageScript.score, // Use the public score variable
            parkFeedbackText,
            parkHighFeedback,
            parkLowFeedback,
            12,
            "Park"
        );
    }

    private void EvaluateStage(
        int score,
        TextMeshProUGUI feedbackText,
        string highFeedback,
        string lowFeedback,
        int threshold,
        string stageName
    )
    {
        Debug.Log($"Evaluating {stageName} Stage. Score: {score}");

        if (feedbackText == null)
        {
            Debug.LogError($"Feedback text for {stageName} is not assigned!");
            return;
        }

        // Check if the score is above or below the threshold
        if (score >= threshold)
        {
            feedbackText.text = highFeedback;
            Debug.Log($"{stageName} Stage: High Feedback assigned.");
        }
        else
        {
            feedbackText.text = lowFeedback;
            Debug.Log($"{stageName} Stage: Low Feedback assigned.");
        }
    }

    private void DisplayPlayerName()
    {
        string savedName = "";

        // Get the saved name from the boy's PlayerPrefs
        if (boyPlayerPrefsObject != null)
        {
            savedName = PlayerPrefs.GetString("SavedName", ""); // Default to an empty string if not found
            Debug.Log("Retrieved name from Boy PlayerPrefs: " + savedName);
        }

        // If the boy's name is empty, try getting the girl's name
        if (string.IsNullOrEmpty(savedName) && girlPlayerPrefsObject != null)
        {
            savedName = PlayerPrefs.GetString("SavedName", "No Name"); // Default to "No Name" if not found
            Debug.Log("Retrieved name from Girl PlayerPrefs: " + savedName);
        }

        // Set the name in the assigned Text object
        if (playerNameText != null)
        {
            playerNameText.text = savedName;
            Debug.Log($"Player Name Loaded: {savedName}");
        }
        else
        {
            Debug.LogWarning("Player Name Text is not assigned!");
        }
    }
}
