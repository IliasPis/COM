using UnityEngine;
using TMPro;

public class SavePrefs : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI nameText;

    private const string PlayerPrefKey = "SavedName";

    public void SaveName()
    {
        if (inputField != null)
        {
            string nameToSave = inputField.text;
            PlayerPrefs.SetString(PlayerPrefKey, nameToSave);
            PlayerPrefs.Save();
            Debug.Log("Name saved: " + nameToSave);
        }
    }

    public void SetNameText()
    {
        if (nameText != null)
        {
            string savedName = PlayerPrefs.GetString(PlayerPrefKey, "No Name");
            nameText.text = savedName;
            Debug.Log("Name loaded: " + savedName);
        }
    }

    public void ResetInputField()
    {
        // Clear the input field
        if (inputField != null)
        {
            inputField.text = "";
        }

        // Clear the displayed name text
        if (nameText != null)
        {
            nameText.text = "";
        }

        Debug.Log("Input field and name text cleared.");
    }
}
