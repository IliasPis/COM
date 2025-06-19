using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DragDropQuizWords : MonoBehaviour
{
    public TextMeshProUGUI[] words; // Text items to drag
    public TextMeshProUGUI[] dropZones; // Drop zones where words will be combined
    private string[] originalWords; // Initial text of each draggable word
    private string[] originalDropZoneTexts; // Initial text of each drop zone
    private Vector3[] originalPositions; // Initial positions of draggable items

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI outcomeText;

    public GameObject UI;
    public GameObject Content;
    public GameObject outcome;

    public AudioClip dropSound; // Sound to play on item drop
    private AudioSource audioSource; // AudioSource for playing sounds

    private int score;

    private string uniqueID; // Unique identifier for this instance

    void Awake()
    {
        uniqueID = gameObject.name + "_" + GetInstanceID(); // Assign a unique ID
        Debug.Log($"Initialized instance with ID: {uniqueID}");

        if (words.Length == 0 || dropZones.Length == 0)
        {
            Debug.LogError($"GameObject {uniqueID} is missing required references!");
        }

        // Attach or find AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        // Ensure clean initialization
        score = 0;
        originalPositions = new Vector3[words.Length];
        originalWords = new string[words.Length];
        originalDropZoneTexts = new string[dropZones.Length];

        for (int i = 0; i < words.Length; i++)
        {
            originalPositions[i] = words[i].transform.position;
            originalWords[i] = words[i].text;
        }

        for (int i = 0; i < dropZones.Length; i++)
        {
            originalDropZoneTexts[i] = dropZones[i].text;
        }

        ResetState();
        UpdateScore();
    }

    private void ResetState()
    {
        // Reset draggable words
        for (int i = 0; i < words.Length; i++)
        {
            words[i].transform.position = originalPositions[i];
            words[i].text = originalWords[i];
            words[i].gameObject.SetActive(true);
        }

        // Reset drop zones
        for (int i = 0; i < dropZones.Length; i++)
        {
            dropZones[i].text = originalDropZoneTexts[i];
            dropZones[i].gameObject.SetActive(true);
        }

        // Reset UI
        UI.SetActive(true);
        Content.SetActive(true);
        outcome.SetActive(false);
        outcomeText.gameObject.SetActive(false);
    }

    public void OnItemDropped(GameObject draggedItem)
    {
        TextMeshProUGUI draggedText = draggedItem.GetComponent<TextMeshProUGUI>();
        int wordIndex = System.Array.IndexOf(words, draggedText);
        if (wordIndex < 0 || wordIndex >= words.Length) return;

        foreach (TextMeshProUGUI dropZone in dropZones)
        {
            RectTransform dropZoneRect = dropZone.GetComponent<RectTransform>();
            RectTransform draggedItemRect = draggedText.GetComponent<RectTransform>();

            if (IsRectOverlapping(dropZoneRect, draggedItemRect))
            {
                int dropZoneIndex = System.Array.IndexOf(dropZones, dropZone);
                if (wordIndex == dropZoneIndex)
                {
                    score++;
                    UpdateScore();
                }

                CombineWords(draggedText, dropZone);
                draggedText.gameObject.SetActive(false);

                // Play sound on successful drop
                PlayDropSound();

                CheckCompletion();
                break;
            }
        }
    }

    private void CombineWords(TextMeshProUGUI draggedText, TextMeshProUGUI dropZone)
    {
        dropZone.text += draggedText.text;
    }

    private void PlayDropSound()
    {
        if (dropSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(dropSound);
        }
    }

    private bool IsRectOverlapping(RectTransform rect1, RectTransform rect2)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rect1, rect2.position, null);
    }

    private void CheckCompletion()
    {
        foreach (TextMeshProUGUI word in words)
        {
            if (word.gameObject.activeSelf) return;
        }

        StartCoroutine(WaitAndCompleteQuiz());
    }

    private IEnumerator WaitAndCompleteQuiz()
    {
        yield return new WaitForSeconds(1f);

        foreach (TextMeshProUGUI word in words) word.gameObject.SetActive(false);
        foreach (TextMeshProUGUI dropZone in dropZones) dropZone.gameObject.SetActive(false);

        UI.SetActive(false);
        Content.SetActive(false);
        outcome.SetActive(true);

        DisplayOutcome();
    }

    private void DisplayOutcome()
    {
        outcomeText.gameObject.SetActive(true);
        outcomeText.text = "Final Score: " + score;
    }

    public void TryAgain()
    {
        ResetState();
        score = 0;
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }
}
