using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    // Use static to ensure the timer keeps running even if the object is disabled
    public static float TimePlayed;

    public TextMeshProUGUI TimeDisplay;

    void Start()
    {
        // Optionally, initialize the timer here, but it will already start counting on game start
        // TimePlayed = 0f; // If you want to reset the timer when the game starts
    }

    void Update()
    {
        // Increment the time played
        TimePlayed += Time.deltaTime;

        // Calculate hours, minutes, and seconds
        int hours = Mathf.FloorToInt(TimePlayed / 3600); // 1 hour = 3600 seconds
        int minutes = Mathf.FloorToInt((TimePlayed % 3600) / 60); // Remaining minutes
        int seconds = Mathf.FloorToInt(TimePlayed % 60); // Remaining seconds

        // Format the time as "00 h 00 m 00 sec"
        string formattedTime = string.Format("{0:00} h {1:00} m {2:00} sec", hours, minutes, seconds);

        // Display the formatted time if TimeDisplay is assigned (UI might not always exist)
        if (TimeDisplay != null)
        {
            TimeDisplay.SetText(formattedTime);
        }
    }
}
