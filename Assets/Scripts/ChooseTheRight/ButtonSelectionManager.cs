using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectionManager : MonoBehaviour
{
    // Public references to the buttons
    public Button button1;
    public Button button2;
    public Button nextButton;

    // Keeps track of the selected button
    private Button selectedButton = null;

    // Shared score variable
    public static int totalScore = 0;

    void Start()
    {
        // Add listeners for button selection
        button1.onClick.AddListener(() => SelectButton(button1));
        button2.onClick.AddListener(() => SelectButton(button2));
        
        // Add listener for the next button
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    private void SelectButton(Button button)
    {
        selectedButton = button;
        Debug.Log("Button Selected: " + button.name);
    }

    private void OnNextButtonClicked()
    {
        if (selectedButton != null)
        {
            totalScore += 1; // Increment score
            Debug.Log("Correct choice! Total Score: " + totalScore);
        }
        else
        {
            Debug.Log("No button selected. Total Score: " + totalScore);
        }

        // Reset selected button for the next round
        selectedButton = null;
    }
}
