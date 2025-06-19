using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DragDropQuiz_Locations : MonoBehaviour
{
    public GameObject[] actions; // Items to drag
    public List<GameObject> cafeteriaItemsCorrect; // Items that belong to Cafeteria
    public List<GameObject> gardenItemsCorrect; // Items that belong to School Garden
    public List<GameObject> hallwayItemsCorrect; // Items that belong to Hallway near classrooms
    public List<GameObject> playgroundItemsCorrect; // Items that belong to Playground area

    public Button[] cafeteriaLocation; // Cafeteria drop zones
    public Button[] gardenLocation; // School Garden drop zones
    public Button[] hallwayLocation; // Hallway near classrooms drop zones
    public Button[] playgroundLocation; // Playground area drop zones
    private Vector3[] originalPositions; // Initial positions of draggable items

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI outcomeText;

    public GameObject UI;
    public GameObject Content;
    public GameObject outcome;

    public AudioClip dropSound; // Sound to play on item drop
    public Color highlightColor = Color.yellow; // Highlight color for locations
    private AudioSource audioSource;

    private List<GameObject> cafeteriaDropped = new List<GameObject>();
    private List<GameObject> gardenDropped = new List<GameObject>();
    private List<GameObject> hallwayDropped = new List<GameObject>();
    private List<GameObject> playgroundDropped = new List<GameObject>();

    public int score;

    void Start()
    {
        originalPositions = new Vector3[actions.Length];

        for (int i = 0; i < actions.Length; i++)
        {
            originalPositions[i] = actions[i].transform.position;
        }

        outcomeText.gameObject.SetActive(false);

        // Attach or find AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void OnItemDropped(GameObject draggedItem)
    {
        bool isDropped = false;

        // Check for Cafeteria Location
        foreach (Button location in cafeteriaLocation)
        {
            if (IsOverlapping(draggedItem, location.gameObject))
            {
                HandleItemDrop(draggedItem, cafeteriaDropped, location);
                isDropped = true;
                break;
            }
        }

        // Check for School Garden Location
        if (!isDropped)
        {
            foreach (Button location in gardenLocation)
            {
                if (IsOverlapping(draggedItem, location.gameObject))
                {
                    HandleItemDrop(draggedItem, gardenDropped, location);
                    isDropped = true;
                    break;
                }
            }
        }

        // Check for Hallway Location
        if (!isDropped)
        {
            foreach (Button location in hallwayLocation)
            {
                if (IsOverlapping(draggedItem, location.gameObject))
                {
                    HandleItemDrop(draggedItem, hallwayDropped, location);
                    isDropped = true;
                    break;
                }
            }
        }

        // Check for Playground Location
        if (!isDropped)
        {
            foreach (Button location in playgroundLocation)
            {
                if (IsOverlapping(draggedItem, location.gameObject))
                {
                    HandleItemDrop(draggedItem, playgroundDropped, location);
                    isDropped = true;
                    break;
                }
            }
        }

        // If not dropped in any location, reset to original position
        if (!isDropped)
        {
            int actionIndex = System.Array.IndexOf(actions, draggedItem);
            if (actionIndex >= 0 && actionIndex < actions.Length)
            {
                draggedItem.transform.position = originalPositions[actionIndex];
            }
        }

        // Check if all items have been dropped
        CheckCompletion();
    }

    private void HandleItemDrop(GameObject draggedItem, List<GameObject> dropList, Button location)
    {
        if (!dropList.Contains(draggedItem))
        {
            dropList.Add(draggedItem);
        }

        draggedItem.transform.position = location.transform.position;
        draggedItem.SetActive(false); // Deactivate the item

        // Play sound and highlight the location
        PlayDropSound();
        StartCoroutine(HighlightLocation(location));
    }

    private void PlayDropSound()
    {
        if (dropSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(dropSound);
        }
    }

    private IEnumerator HighlightLocation(Button location)
    {
        Color originalColor = location.image.color; // Save the original color
        location.image.color = highlightColor; // Change to highlight color

        yield return new WaitForSeconds(2f); // Wait for 2 seconds

        location.image.color = originalColor; // Revert to the original color
    }

    private bool IsOverlapping(GameObject draggedItem, GameObject location)
    {
        RectTransform locationRect = location.GetComponent<RectTransform>();
        RectTransform draggedItemRect = draggedItem.GetComponent<RectTransform>();

        return IsRectOverlapping(locationRect, draggedItemRect);
    }

    private bool IsRectOverlapping(RectTransform rect1, RectTransform rect2)
    {
        Vector3[] corners1 = new Vector3[4];
        Vector3[] corners2 = new Vector3[4];

        rect1.GetWorldCorners(corners1);
        rect2.GetWorldCorners(corners2);

        Rect rect1Bounds = new Rect(corners1[0].x, corners1[0].y, corners1[2].x - corners1[0].x, corners1[2].y - corners1[0].y);
        Rect rect2Bounds = new Rect(corners2[0].x, corners2[0].y, corners2[2].x - corners2[0].x, corners2[2].y - corners2[0].y);

        return rect1Bounds.Overlaps(rect2Bounds);
    }

    private void CheckCompletion()
    {
        foreach (GameObject action in actions)
        {
            if (action.activeSelf) return;
        }

        CalculateScore();
    }

    public void CalculateScore()
    {
        score = 0;

        foreach (GameObject item in cafeteriaDropped)
        {
            if (cafeteriaItemsCorrect.Contains(item))
            {
                score += 1;
            }
        }

        foreach (GameObject item in gardenDropped)
        {
            if (gardenItemsCorrect.Contains(item))
            {
                score += 1;
            }
        }

        foreach (GameObject item in hallwayDropped)
        {
            if (hallwayItemsCorrect.Contains(item))
            {
                score += 1;
            }
        }

        foreach (GameObject item in playgroundDropped)
        {
            if (playgroundItemsCorrect.Contains(item))
            {
                score += 1;
            }
        }

        DisplayOutcome();
    }

    void DisplayOutcome()
    {
        outcomeText.gameObject.SetActive(true);
        outcomeText.text = "Final Score: " + score;

        UI.SetActive(false);
        Content.SetActive(false);
        outcome.SetActive(true);
    }

    public void TryAgain()
    {
        score = 0;

        cafeteriaDropped.Clear();
        gardenDropped.Clear();
        hallwayDropped.Clear();
        playgroundDropped.Clear();

        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].transform.position = originalPositions[i];
            actions[i].SetActive(true);
        }

        UI.SetActive(true);
        Content.SetActive(true);
        outcome.SetActive(false);
    }
}
