using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DragDropQuiz : MonoBehaviour
{
    public GameObject[] actions; // Items to drag
    public List<GameObject> garbageItemsCorrect; // Items for Garbage Bin
    public List<GameObject> recycleItemsCorrect; // Items for Recycle Bin
    public List<GameObject> compostItemsCorrect; // Items for Compost Bin

    public Button[] garbageBin; // Garbage bin drop zones
    public Button[] recycleBin; // Recycle bin drop zones
    public Button[] compostBin; // Compost bin drop zones
    private Vector3[] originalPositions; // Initial positions of draggable items

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI outcomeText;

    public GameObject UI;
    public GameObject Content;
    public GameObject outcome;

    public int score;

    // Sound and highlight features
    public AudioClip dropSound; // Sound to play on item drop
    public Color highlightColor = Color.yellow; // Highlight color for the bin
    private AudioSource audioSource; // AudioSource component for playing sounds

    private List<GameObject> garbageDropped = new List<GameObject>();
    private List<GameObject> recycleDropped = new List<GameObject>();
    private List<GameObject> compostDropped = new List<GameObject>();

    void Start()
    {
        originalPositions = new Vector3[actions.Length];
        for (int i = 0; i < actions.Length; i++)
        {
            originalPositions[i] = actions[i].transform.position;
        }

        // Attach or find AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        outcomeText.gameObject.SetActive(false);
    }

    public void OnItemDropped(GameObject draggedItem)
    {
        bool isDropped = false;

        // Check for Garbage Bin
        foreach (Button bin in garbageBin)
        {
            if (IsOverlapping(draggedItem, bin.gameObject))
            {
                HandleItemDrop(draggedItem, garbageDropped, bin);
                isDropped = true;
                break;
            }
        }

        // Check for Recycle Bin
        if (!isDropped)
        {
            foreach (Button bin in recycleBin)
            {
                if (IsOverlapping(draggedItem, bin.gameObject))
                {
                    HandleItemDrop(draggedItem, recycleDropped, bin);
                    isDropped = true;
                    break;
                }
            }
        }

        // Check for Compost Bin
        if (!isDropped)
        {
            foreach (Button bin in compostBin)
            {
                if (IsOverlapping(draggedItem, bin.gameObject))
                {
                    HandleItemDrop(draggedItem, compostDropped, bin);
                    isDropped = true;
                    break;
                }
            }
        }

        // If not dropped in any bin, reset to original position
        if (!isDropped)
        {
            int actionIndex = System.Array.IndexOf(actions, draggedItem);
            if (actionIndex >= 0 && actionIndex < actions.Length)
            {
                draggedItem.transform.position = originalPositions[actionIndex];
            }
        }

        CheckCompletion();
    }

    private void HandleItemDrop(GameObject draggedItem, List<GameObject> dropList, Button bin)
    {
        if (!dropList.Contains(draggedItem))
        {
            dropList.Add(draggedItem);
        }

        draggedItem.transform.position = bin.transform.position;
        draggedItem.SetActive(false); // Deactivate the item

        PlayDropSound(); // Play sound
        StartCoroutine(HighlightBin(bin)); // Highlight bin
    }

    private void PlayDropSound()
    {
        if (dropSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(dropSound);
        }
    }

    private IEnumerator HighlightBin(Button bin)
    {
        Color originalColor = bin.image.color; // Save the original color
        bin.image.color = highlightColor; // Change to highlight color

        yield return new WaitForSeconds(2f); // Wait for 2 seconds

        bin.image.color = originalColor; // Revert to the original color
    }

    private bool IsOverlapping(GameObject draggedItem, GameObject bin)
    {
        RectTransform binRect = bin.GetComponent<RectTransform>();
        RectTransform draggedItemRect = draggedItem.GetComponent<RectTransform>();

        return IsRectOverlapping(binRect, draggedItemRect);
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
            if (action.activeSelf) // If any item is still active, return early
            {
                return;
            }
        }

        CalculateScore();
    }

    public void CalculateScore()
    {
        score = 0;

        foreach (GameObject item in garbageDropped)
        {
            if (garbageItemsCorrect.Contains(item))
            {
                score += 1;
            }
        }

        foreach (GameObject item in recycleDropped)
        {
            if (recycleItemsCorrect.Contains(item))
            {
                score += 1;
            }
        }

        foreach (GameObject item in compostDropped)
        {
            if (compostItemsCorrect.Contains(item))
            {
                score += 2;
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

        garbageDropped.Clear();
        recycleDropped.Clear();
        compostDropped.Clear();

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
