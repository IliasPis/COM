using UnityEngine;
using TMPro;

public class SaveNickname : MonoBehaviour
{
    public TMP_InputField inputNicknameBoy;
    public TMP_InputField inputNicknameGirl;
    public TextMeshProUGUI displayNickname;

    public void SaveName()
    {
        // Get the nickname from input fields, prioritizing the boy's field if both are filled
        string nickname = string.IsNullOrWhiteSpace(inputNicknameBoy.text) ? inputNicknameGirl.text : inputNicknameBoy.text;

        // If no input is provided, set it to an empty string
        nickname = string.IsNullOrWhiteSpace(nickname) ? "" : nickname;

        // Save the nickname to PlayerPrefs
        PlayerPrefs.SetString("Nickname", nickname);
        PlayerPrefs.Save();

        // Update the display
        if (displayNickname != null)
        {
            displayNickname.text = nickname;
        }

        Debug.Log("Nickname saved: " + nickname);
    }

    public void ResetInputs()
    {
        // Clear the input fields
        if (inputNicknameBoy != null)
        {
            inputNicknameBoy.text = "";
        }

        if (inputNicknameGirl != null)
        {
            inputNicknameGirl.text = "";
        }

        // Clear the displayed nickname
        if (displayNickname != null)
        {
            displayNickname.text = "";
        }

        Debug.Log("Nickname inputs cleared.");
    }
}
